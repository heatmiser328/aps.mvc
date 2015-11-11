angular.module('apsServices', [])
.factory('PayrollService', ['$log', '$http', '$q', function ($log, $http, $q) {
    $log.info('Loading payroll service');    

    function fetch(date) {        
        $log.debug('retrieve payroll for ' + date);
        var deferred = $q.defer();
        $http.get('http://localhost:52897/api/payroll/start/' + date)
        .then(function (resp) {
            var payroll = [];
            $log.debug('retrieved payroll');
            $log.debug(resp);
            payroll = resp.data;
            
            deferred.resolve(payroll);
        },
        function (resp) {
            deferred.reject(resp);            
        });
        return deferred.promise;
    }
    return {
        fetch: fetch
    };
}]);

