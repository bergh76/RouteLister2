(function () {
    'use strict';

    var controllerId = 'routeListController';

    angular.module('RouteListApp').controller(controllerId,
        ['$scope', 'routeListFactory', routeListController]);

    function routeListController($scope, routeListFactory) {
        $scope.routeList = null;

        routeListFactory.getRouteList().success(function (data) {
            $scope.routeList = data;
        }).error(function (error) {
            // log errors
        });
    }
})();
