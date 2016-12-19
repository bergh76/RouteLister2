(function () {
    'use strict';

    var controllerId = 'routeListController';

    angular.module('RouteListApp').controller(controllerId,
        ['$scope', 'routeListFactory', routeListController]);

    function routeListController($scope, routeListFactory) {
        $scope.routeList = null;
        $scope.orders = null;
        $scope.orderRows = null;


        routeListFactory.getRouteList().success(function (data) {
            //Load data
            $scope.routeList = data;
            //$scope.orders = $scope.routeList.orders;
            //$scope.orderRows = $scope.orders.orderRows
            //get connected!
        }).error(function (error) {
            // log errors
        });
   
    }
})();
