angular.module('apsLogin', ['ui.router'])
.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $stateProvider
    .state('loginApp', {
        url: '/',
        templateUrl: 'login',
        controller: 'LoginCtrl'
    });

    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/');
}])
.controller('LoginCtrl', ['$scope', '$log', '$window', '$http', function ($scope, $log, $window, $http) {

    $scope.login = function () {
        $log.info('Logging in ' + $scope.username);
        $scope.loginError = false;
        $http.post('/Account/Login', {
            Email: $scope.username,
            Password: $scope.password,
            RememberMe: $scope.remember
        })
        .success(function (result) {
            $log.info('successfully logged in');
            $window.location.href = '/';
        })
        .error(function () {
            $log.error('try again!');
            $scope.loginError = true;
        });
    }
}]);

