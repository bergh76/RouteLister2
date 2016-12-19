(function () {
    'use strict';

    var serviceId = 'routeListService';

    angular.module('RouteListApp').service(serviceId,
        ['$http', routeListFactory]);

    function routeListFactory($http) {

        function getRouteList() {
            return $http.get('/api/v1/RouteList');
        }

        var service = {
            getRouteList: getRouteList
        };

        return service;
    }
})();