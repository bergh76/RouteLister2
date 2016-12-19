(function () {
    'use strict';

    var serviceId = 'orderFactory';

    angular.module('RouteListApp').factory(serviceId,
        ['$http', orderFactory]);

    function orderFactory($http) {

        function getOrders(routeListId) {
            return $http.get('/api/v1/order/'+routeListId);
        }

        var service = {
            getOrders: getOrders
        };

        return service;
    }
})();