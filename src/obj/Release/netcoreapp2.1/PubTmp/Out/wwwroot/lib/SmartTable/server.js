'use strict';

// Imitation of server - for testing purpose
angular.module('serverSide', ['ngMockE2E'])
    .run(['$httpBackend', '$http', function ($httpBackend, $http) {
            var users = [
            { "Name":"Ece","DamageCost": 13, "Year": 1991}];

   
        } ]);