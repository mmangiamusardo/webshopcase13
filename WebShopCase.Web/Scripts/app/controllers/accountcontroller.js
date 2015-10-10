angular.module("wscApp")
    .controller('AccountCtrl', function ($scope, $http, $location, tokenKey, srvAccount, srvProfile, srvSharedData, toaster) {

        $scope.spinning = false;
        $scope.errorMessage = '';

        var successRegistrationCallback = function (data, status, headers, config) {
            $scope.spinning = false;
            toaster.pop({
                type: 'success',
                body: "User Registration Successful. Please SignIn :)",
                onHideCallback: function () {
                    $location.path('/signin');
                }
            });
        }

        var errorRegistrationCallback = function (result, status, headers, config) {
            $scope.spinning = false;
            console.log(result);
      
            for (var i = 0; i < result.length; i++) {
                $scope.errorMessage += result[i] + "</br>"
            }

            toaster.pop({
                type: 'error',
                body: $scope.errorMessage,
                bodyOutputType: 'trustedHtml',
            });

        }

        var successLoginCallback = function (data, status, headers, config) {
            $scope.spinning = false;
            sessionStorage.setItem(tokenKey, data.access_token);
            $scope.isAuthenticated = true;

            srvProfile
                .get($scope.user.Email)
                    .success(function (data, status, headers, config) {
                        srvSharedData.setCustomer(data);
                        toaster.pop({
                            type: 'success',
                            body: $scope.user.Email + " logged in :)",
                            onHideCallback: function () {
                                $location.path('/');
                            }
                        });

                    })
                    .error(function (result, status, headers, config) {
                        console.log(result);
                        toaster.pop({
                            type: 'error',
                            body: result,
                            bodyOutputType: 'trustedHtml',
                        });
                    });


        };

        var errorLoginCallback = function (data, status, headers, config) {
            $scope.spinning = false;
            console.log(data);
            $scope.errorMessage = data.error_description;

            toaster.pop({
                type: 'error',
                body: $scope.errorMessage,
                bodyOutputType: 'trustedHtml',
            });
        }

        $scope.register = function () {
            $scope.spinning = true;
            srvAccount
                .register(JSON.stringify($scope.user))
                    .success(successRegistrationCallback)
                        .error(errorRegistrationCallback);

            $scope.errorMessage = '';
        };

        $scope.signin = function () {
            $scope.spinning = true;

            var loginData = {
                grant_type: 'password',
                username: $scope.user.Email,
                password: $scope.user.Password
            };

            srvAccount.generateAccessToken(loginData)
                .success(successLoginCallback)
                    .error(errorLoginCallback);

            $scope.errorMessage = '';
        };
});