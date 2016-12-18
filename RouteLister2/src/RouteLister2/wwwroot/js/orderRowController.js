(function () {
    'use strict';

    var controllerId = 'orderRowController';

    angular.module('RouteListApp').controller(controllerId,
        ['$scope', 'orderRowFactory', orderRowController]);

    function orderRowController($scope, orderRowFactory) {
        $scope.orderRows = [];

        orderRowFactory.getOrderRows().success(function (data) {
            $scope.orderRows = data;
        }).error(function (error) {
            // log errors
        });
    }
})();