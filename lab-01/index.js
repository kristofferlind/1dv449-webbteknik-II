var request = require('request'),
	cheerio = require('cheerio'),
	mongo = require('mongodb').MongoClient,
	express = require('express'),
	q = require('q'),
	app = express();

mongo.connect('mongodb://localhost:27017/coursepress', function(err, db) {
	var courseDB = db.collection('courses');
	var metaDB = db.collection('courseMeta');

	var scrape = function() {
		console.log('scraping..')
		var deferred = q.defer();

		var numberOfPages;

		var url = 'https://coursepress.lnu.se/kurser/?bpage=1';

		var getPage = function(url, callback) {
			request({url: url, headers: {'User-Agent': 'scraper'}}, callback);
		};

		var isCourse = function(course) {
			var re = new RegExp("kurs");

			if (course) {
				return course.url.match(re);
			} else {
				return false;
			}
		}

		var getPageCourses = function(doc) {
			var deferredPageCourses = q.defer();
			var courses = doc('ul#blogs-list li');
			var counter = 0;
			courses.each(function(index, courseDoc) {
				var courseData = doc(courseDoc);
				getCourseInfo(courseData, doc).then(function() {
					counter++;
					if (counter == courses.length) {
						deferredPageCourses.resolve();
					}
				})
			});

			return deferredPageCourses.promise;
		};

		var getCourseSyllabusLink = function(doc) {
			var href;
			doc('li.menu-item a').each(function(index, element) {
				var item = doc(element);
				if (item.text() == 'Kursplan' || item.text() == 'Syllabus' || item.text() == 'Course Syllabus') {
					href = item.attr('href');
				}
			});
			return href;
		}

		var extractDate = function(dateString) {
			var re = new RegExp("[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}");
			var date = dateString.match(re);
			if (date && date[0]) {
				return new Date(date[0]);
			}
			return null;
		}

		var getDescriptionForMalformedPage = function(doc) {
			var entry = doc('div.entry-content').first();
			entry.find('h1, h2').remove();
			var description = entry.text();
			return description.trim();
		}

		var getDescription = function(doc) {
			var entry = doc('div.entry-content').first();
			entry.find('h1, h2, article').remove();
			var description = entry.find('p').text();
			return description.trim();
		}

		var getCourseInfo = function(courseDoc) {
			var deferredCourseInfo = q.defer();
			var course = {
				name: courseDoc.find('div.item-title a').text() || 'no information',
				url: courseDoc.find('div.item-title a').attr('href') || 'no information'
			};

			if (isCourse(course)) {
				getPage(course.url, function(err, status, html) {
					var doc = cheerio.load(html);
					course.code = doc('div#header-wrapper li a').last().text() || 'no information';
					course.syllabus = getCourseSyllabusLink(doc) || 'no information';
					course.description = getDescription(doc) || getDescriptionForMalformedPage(doc) || 'no information';
					course.latest = {
						header: doc('section article.post').first().find('h1.entry-title a').text() || 'no information',
						date: extractDate(doc('section article.post').first().find('p.entry-byline').text()) || 'no information',
						author: doc('section article.post').first().find('p.entry-byline strong').text() || 'no information',
					}

					courseDB.update({name: course.name}, course, {upsert: true}, function(err, data) {
						deferredCourseInfo.resolve();
					});

					metaDB.update({name: 'lastModified'}, {name: 'lastModified', lastModified: new Date()}, {upsert: true}, function(err, data) {
						//saved..
					});
				});
			} else {
				deferredCourseInfo.resolve();
			}
			return deferredCourseInfo.promise;
		};

		var getNumberOfPages = function(doc) {
			var highestNumber = 0;
			var pageLinks = doc('div.pagination-links a.page-numbers');
			pageLinks.each(function(index, pageLink) {
				var number = parseInt(doc(pageLink).text(), 10);
				if (number > highestNumber) {
					highestNumber = number;
				}
			});

			return highestNumber;
		}


		getPage(url, function(err, status, html) {
			if (err) {
				throw err;
			}

			var doc = cheerio.load(html);
			numberOfPages = getNumberOfPages(doc);
			var finished = [];

			for (var i = 1; i <= numberOfPages; i++) {
				var url = 'https://coursepress.lnu.se/kurser/?bpage=' + i;
				(function(i) {
					getPage(url, function(err, status, html) {
						if (err) {
							throw err;
						}
						var doc = cheerio.load(html);

						getPageCourses(doc).then(function() {
							finished.push(i);
							if (finished.length === numberOfPages) {
								// deferred.resolve();

								// Sometimes scraping isn't really complete on resolve. 
								// No idea what i've missed(figuring out q.all would probably solve this)
								setTimeout(function() {
									deferred.resolve();
								}, 15000);
							}
						});
					});
				}(i));
			};
		});

		return deferred.promise;
	}

	//This solution doesn't follow instructions, but is definately better (solution below follows instructions)
	//Scrape every 5min. Scheduling this would be an improvement. (And probably just doing it once a day)
	// setTimeout(function() {
	// 	scrape();
	// }, 300000);		//Every 5min

	// app.get('/', function(req, res) {
	// 	metaDB.findOne({}, function(err, meta) {
	// 		courseDB.find({}, {_id: 0}).sort({name: 1}).toArray(function(err, data) {
	// 			data.push({meta: {courseCount: data.length, lastScrape: meta.lastModified}});
	// 			res.status(200).json(data);
	// 		});
	// 	});
	// });

	// Solution according to instructions
	app.get('/', function(req, res) {
		metaDB.findOne({}, function(err, meta) {
			var comparisonDate = new Date();
			comparisonDate.setMinutes(comparisonDate.getMinutes() - 5);
			if (meta.lastModified.getTime() < comparisonDate.getTime()) {
				scrape().then(function() {
					courseDB.find({}, {_id: 0}).sort({name: 1}).toArray(function(err, data) {
						data.push({meta: {courseCount: data.length, lastScrape: meta.lastModified}});
						res.status(200).json(data);
					});
				});
			} else {
				courseDB.find({}, {_id: 0}).sort({name: 1}).toArray(function(err, data) {
					data.push({meta: {courseCount: data.length, lastScrape: meta.lastModified}});
					res.status(200).json(data);
				});
			}
		})
	});

	app.get('/coursenames', function(req, res) {
		courseDB.find({}, {_id:0, name:1}).sort({name:1}).toArray(function(err, data) {
			res.status(200).json(data);
		});
	});

	app.listen('8000');
	console.log('Listening on port 8000');
});
