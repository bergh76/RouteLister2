(function () {
    'use strict';

    var serviceId = 'orderRowFactory';

    angular.module('RouteListApp').factory(serviceId,
        ['$http', orderRowFactory]);

    function orderRowFactory($http) {

        function getOrderRows() {
            return $http.get('/api/v1/orderRow');
        }

        var service = {
            getOrderRows: getOrderRows
        };

        return service;
    }
})();