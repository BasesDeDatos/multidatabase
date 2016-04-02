angular.module('create', [])
    .controller('crateCtrl', ['$scope', '$http', function ($scope, $http) {
        $scope.addDatabase = function () {
            $scope.showMessage = false;

            var params = {
                name: $scope.name,
            };

            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/createDatabase',
                method: "POST",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
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