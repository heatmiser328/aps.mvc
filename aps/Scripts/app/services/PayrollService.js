angular.module('apsServices', [])
.factory('PayrollService', ['$log', '$http', '$q', 'apiURL', function ($log, $http, $q, apiURL) {
    $log.info('Loading payroll service');    

    function fetch(date) {        
        $log.debug('retrieve payroll for ' + date);
        var deferred = $q.defer();
        $http.get(apiURL + '/payroll/start/' + date)
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

    function save(payroll) {
        $log.debug('save payroll for ' + payroll.StartTDS);
        var deferred = $q.defer();        
        $http.post(apiURL + '/payroll', payroll)
        .then(function (resp) {            
            $log.debug('saved payroll');
            $log.debug(resp);
            deferred.resolve(resp);
        },
        function (resp) {
            deferred.reject(resp);
        });
        return deferred.promise;
    }
    function setDailyGross(employee, idx, newValue) {
        employee.Grosses[idx].Gross = newValue;
        employee.Grosses[idx].GrossTDS = new Date();
        employee.Grosses[idx].Dirty = true;
        employee.Grosses[idx].Modified = new Date();
        //employee.Grosses[idx].ModifiedBy = ;
    }
    function calculateGross(employee) {
        return _.sum(employee.Grosses, function (gross) {
            return gross.Gross;
        });
    }
    function calculateRent(employee) {
        return employee.Gross * employee.RentRate;
    }
    function calculateNet(employee) {
        return employee.Gross * (1 - employee.RentRate);
    }
    return {
        fetch: fetch,
        save: save,
        setDailyGross: setDailyGross,
        calculateGross: calculateGross,
        calculateRent: calculateRent,
        calculateNet: calculateNet
    };
}]);

