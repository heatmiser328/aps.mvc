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
    $scope.date = new Date();

    function load() {
        var date = $scope.date.getFullYear() + '-' + ($scope.date.getMonth() + 1) + '-0' + $scope.date.getDate();
                
        PayrollService.fetch(date)
        .then(function (data) {
            $log.debug('retrieved payroll');
            $log.debug(data);
            
            $scope.gridOptions.columnDefs = [
               { name: 'name', enableCellEdit: false }
            ];
            
            var payroll = _.map(data.Employees, function (emp, j) {
                var o = {
                    name: emp.Employee.FullName,
                };
                _.each(emp.Grosses, function (g, i) {
                    if (j == 0) {
                        var s = $filter('date')(g.GrossDate, 'MMM-dd');
                        $scope.gridOptions.columnDefs.push({ name: i.toString(), displayName: s, enableCellEdit: true, type: 'number' });
                    }
                    o[i.toString()] = g.GrossPay;
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
    load();
}]);

