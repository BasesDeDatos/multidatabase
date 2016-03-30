angular.module('manager', [])
    .controller('managerCtrl', ['$scope', '$http', function ($scope, $http) {
        $scope.addDatabase = function () {
            $scope.showMessage = false;

            var params = {
                database_type: $scope.database_type,
                user: $scope.user,
                pass: $scope.pass,
                server: $scope.server,
                protocol: $scope.protocol,
                port: $scope.protocol,
                alias: $scope.alias
            };

            $http({
                url: 'http://localhost:54607/ServiceAPI.svc/addDatabase',
                method: "POST",
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(result));
                alert("revise la consola");
            })
            .error(function (data, status, headers, config) {
                $scope.message = data.error_description.replace(/["']{1}/gi, "");
                $scope.showMessage = true;
                console.log(JSON.stringify(result));
                alert("revise la consola");
            });
        }
    }]);