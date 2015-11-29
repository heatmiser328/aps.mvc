angular.module('apsControllers', [])
.controller('PayrollCtrl', ['$scope', '$log', '$filter', 'PayrollService', function ($scope, $log, $filter, PayrollService) {
    $log.info('Loading payroll controller');
    $scope.gridOptions = {};
    $scope.gridOptions.onRegisterApi = function (gridApi) {
        //set gridApi on scope
        $scope.gridApi = gridApi;
        gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
            $log.debug('edited row id:' + rowEntity.id + ' Column:' + colDef.name + ' newValue:' + newValue + ' oldValue:' + oldValue);
            $log.debug(rowEntity);
            //$scope.$apply();
        });
    };    
    
    $scope.dt = new Date();
    $scope.payrollDate = '';
    $scope.minDate = moment($scope.dt).add(-5, 'y');
    $scope.maxDate = moment($scope.dt).add(10, 'y');
    $scope.data = {};

    $scope.data.dateOptions = {
        formatYear: 'yy',
        startingDay: 0,
        showWeeks: 'false'
    };

    $scope.$watch('dt', function (dateVal) {
        $log.debug('change date');
        $log.debug(dateVal);
        setPayrollDate(dateVal);
        load();
    });

    function setPayrollDate(dateVal) {
        var weekYear = getWeekNumber(dateVal);
        var year = weekYear[0];
        var week = weekYear[1];
        if (angular.isDefined(week) && angular.isDefined(year)) {
            var weekPeriod = getStartDateOfWeek(week, year);

            /*
            var weekStart = $filter('date')(weekPeriod[0], 'MMM dd, yyyy');
            var weekEnd = $filter('date')(weekPeriod[1], 'MMM dd, yyyy');

            $scope.payrollDate = weekStart + " to " + weekEnd;
            */
            var d = weekPeriod[0];
            d.setDate(d.getDate() + 1);
            $scope.payrollDate = 'Week of ' + $filter('date')(d, 'MMM dd, yyyy');
        }
    }

    function getStartDateOfWeek(w, y) {
        var simple = new Date(y, 0, 1 + (w - 1) * 7);
        var dow = simple.getDay();
        var ISOweekStart = simple;
        if (dow <= 4)
            ISOweekStart.setDate(simple.getDate() - simple.getDay());
        else
            ISOweekStart.setDate(simple.getDate() + 7 - simple.getDay());

        var ISOweekEnd = new Date(ISOweekStart);
        ISOweekEnd.setDate(ISOweekEnd.getDate() + 6);

        return [ISOweekStart, ISOweekEnd];
    }

    function getWeekNumber(d) {
        d = new Date(+d);
        d.setHours(0, 0, 0);
        d.setDate(d.getDate() + 4 - (d.getDay() || 7));
        var yearStart = new Date(d.getFullYear(), 0, 1);
        var weekNo = Math.ceil((((d - yearStart) / 86400000) + 1) / 7);
        return [d.getFullYear(), weekNo];
    }

    function padZero(v) {
        if (v.length < 2) {
            return '0' + v;
        }
        return v;
    }
    function load() {
        var date = $scope.dt.getFullYear() + '-' + ($scope.dt.getMonth() + 1) + '-' + padZero($scope.dt.getDate());
                
        PayrollService.fetch(date)
        .then(function (data) {
            $log.debug('retrieved payroll');
            $log.debug(data);
            
            $scope.gridOptions.columnDefs = [
               { name: 'id', enableCellEdit: false, visible: false},
               { name: 'name', enableCellEdit: false }
            ];
            
            var payroll = _.map(data.Employees, function (emp, j) {
                var o = {
                    id: j,
                    name: emp.Employee.Name,
                };
                _.each(emp.Grosses, function (g, i) {
                    if (j == 0) {

                        var s = $filter('date')(g.GrossTDS, 'EEEE') + ' ' + $filter('date')(g.GrossTDS, 'MMM-dd');
                        $scope.gridOptions.columnDefs.push({ name: i.toString(), displayName: s, enableCellEdit: true, type: 'number' });
                    }
                    o[i.toString()] = g.Gross;
                });
                o.gross = emp.Gross;
                o.net = emp.Net;
                o.rent = emp.Rent;

                return o;
            });
            $scope.gridOptions.columnDefs.push({ name: 'gross', enableCellEdit: false, type: 'number' });
            $scope.gridOptions.columnDefs.push({ name: 'net', enableCellEdit: false, type: 'number' });
            $scope.gridOptions.columnDefs.push({ name: 'rent', enableCellEdit: false, type: 'number' });

            $log.debug($scope.gridOptions);

            $scope.gridOptions.data = payroll;            
        })
        .catch(function (resp) {
            $log.error(resp.status + ' ' + resp.statusText);
        });
    }       
}]);

