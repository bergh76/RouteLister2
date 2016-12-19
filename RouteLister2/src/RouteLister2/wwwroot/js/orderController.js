(function () {
    'use strict';

    var controllerId = 'orderController';

    angular.module('RouteListApp').controller(controllerId,
        ['$scope', 'orderFactory', orderController]);

    function orderController($scope, orderFactory) {
        $scope.orders = [];

        orderFactory.getOrders(routeListId).success(function (data) {
            $scope.orders = data;
        }).error(function (error) {
            // log errors
        });

        function add() {
            $scope.orders.push()
        };
    }
})();