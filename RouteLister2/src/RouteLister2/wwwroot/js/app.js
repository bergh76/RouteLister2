angular.module('RouteListApp', ['ui.router', 'ngResource', 'RouteListApp.controllers', 'RouteListApp.services']);

angular.module('RouteListApp').config(function ($stateProvider) {
    $stateProvider.state('RouteList', { // state for showing all RouteLists
        url: '/RouteList',
        templateUrl: 'partials/RouteLists.html',
        controller: 'RouteListController'
    }).state('viewRouteList', { //state for showing single RouteList
        url: '/RouteList/:id',
        templateUrl: 'partials/RouteList-view.html',
        controller: 'RouteViewController'
    }).state('newRouteList', { //state for adding a new RouteList
        url: '/RouteLists/Create',
        templateUrl: 'partials/RouteList-add.html',
        controller: 'RouteListCreateController'
    }).state('editRouteList', { //state for updating a RouteList
        url: '/RouteLists/Edit/:id',
        templateUrl: 'partials/RouteList-edit.html',
        controller: 'RouteListEditController'
    });
}).run(function ($state) {
    $state.go('RouteLists'); //make a transition to RouteList state when app starts
});