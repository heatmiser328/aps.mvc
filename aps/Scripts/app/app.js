angular.module('apsApp', ['ui.router', 'ui.bootstrap', 'ui.grid', 'ui.grid.edit', 'ui.grid.cellNav', 'apsControllers', 'apsServices'])
.value('apiURL', 'http://localhost:52897/api')
.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $stateProvider
    .state('payroll', {
        url: '/payroll',
        templateUrl: 'PayrollTemplate',
        controller: 'PayrollCtrl'
    })
    .state('employees', {
        url: '/employees',
        templateUrl: 'EmployeesTemplate',
        controller: 'EmployeesCtrl'
    });

    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/payroll');
}])