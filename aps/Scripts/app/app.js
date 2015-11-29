angular.module('apsApp', ['ui.router', 'ui.bootstrap', 'ui.grid', 'ui.grid.edit', 'apsControllers', 'apsServices'])
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