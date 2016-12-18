(function () {
    'use strict';

    var serviceId = 'routeListFactory';

    angular.module('RouteListApp').factory(serviceId,
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