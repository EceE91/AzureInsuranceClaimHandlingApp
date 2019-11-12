'use strict';
// Declare app level module which depends on filters, and services
angular.module('myApp', ['smartTable.table']).
    controller('mainCtrl', ['$scope', '$http', function (scope, $http) {

            scope.rowCollection = [];

            scope.columnCollection = [
                { label: 'Name', map: 'Name', validationAttrs: 'required', validationMsgs: '<span class="error" ng-show="smartTableValidForm.FirstName.$error.required">Required!</span>' },
                { label: 'Damage Cost', map: 'DamageCost', validationAttrs: 'required' },
                { label: 'Year', map: 'Year', validationAttrs: 'required' },
                //{ label: 'Type', map: 'Type', noList: true, editType: 'password' },
                //{ label: 'Customer', map: 'CustId', optionsUrl: '/GetCusts', editType: 'radio' },
                //{ label: 'Type', map: 'RoleId', optionsUrl: '/GetRoles', editType: 'select', defaultTemplate: '<option value="" ng-hide="dataRow[column.map]">---</option>', validationAttrs: 'required', validationMsgs: '<span class="error" ng-show="smartTableValidForm.RoleId.$error.required">Required!</span>' }, // NOTE: small hack which enables defaultTemplate option :)
                //{ label: 'E-mail', editType: 'email', map: 'Email' },
                //{ label: 'Cell Phone', map: 'Mobilephone', noEdit: true, validationAttrs: 'required' },
                //{ label: 'Locked', map: 'IsLocked', cellTemplate: '<input disabled type="checkbox" name="{{column.map}}" ng-model="dataRow[column.map]">', editType: 'checkbox', noAdd: true }
            ];

            $http.get('/Index').success(function (data) {
                debugger;
                $scope.rowCollection = data;
            });

            scope.globalConfig = {
                isPaginationEnabled: true,
                isGlobalSearchActivated: true,
                itemsByPage: 10,
                selectionMode: 'single',
                
            };

        } ]);