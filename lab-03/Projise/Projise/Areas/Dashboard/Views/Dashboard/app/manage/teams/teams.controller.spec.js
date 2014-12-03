describe('Controller: TeamsController', function () {
    'use strict';

    // load the controller's module
    beforeEach(module('projiSeApp'));
    beforeEach(module('socketMock'));

    var TeamsController,
        scope,
        modalInstance = {};

      // Initialize the controller and a mock scope
    beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();
        TeamsController = $controller('TeamsController', {
            $scope: scope,
            $modalInstance: modalInstance
        });
    }));

    it('should be defined', function() {
        expect(TeamsController).toBeDefined();
    });
});
