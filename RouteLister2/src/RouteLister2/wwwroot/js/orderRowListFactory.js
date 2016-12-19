(function () {
    'use strict';

    var serviceId = 'orderRowListFactory';

    angular.module('RouteListApp').factory(serviceId,
        orderRowFactory);

    function orderRowFactory($resource) {


        return $resource('/api/v1/orderRowViewModel/:id');

    };
})();