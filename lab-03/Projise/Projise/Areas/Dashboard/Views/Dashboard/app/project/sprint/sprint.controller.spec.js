describe('Controller: SprintController', function () {
    'use strict';

    // load the controller's module
    beforeEach(module('projiSeApp'));
    beforeEach(module('socketMock'));

    var SprintController,
        scope,
        modalInstance = {};

      // Initialize the controller and a mock scope
    beforeEach(inject(function ($controller, $rootScope) {
        scope = $rootScope.$new();
        SprintController = $controller('SprintController', {
            $scope: scope,
            $modalInstance: modalInstance
        });
    }));

    it('should be defined', function() {
        expect(SprintController).toBeDefined();
    });
});
