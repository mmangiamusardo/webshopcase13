angular.module("wscApp")
    .directive("profileCustomer", function () {
        return {
            restrict: "E",
            templateUrl: "Scripts/app/components/profileCmp.html",
            controller: function ($scope, srvProfile, srvSharedData, srvAccount, $location,toaster) {

                $scope.customer = srvSharedData.getCustomer();

                var successSave = function (data)
                {
                    console.log(data);
                    toaster.pop({
                        type: 'success',
                        body: 'User ' + data + ' successfully saved',
                        bodyOutputType: 'trustedHtml',
                    });
                };

                var failSave = function (error) {
                    console.log(error);
                    var errorMessage = '';

                    for (var i = 0; i < error.length; i++)
                    {
                        errorMessage+= error[i] + "</br>"
                    }

                    toaster.pop({
                        type: 'error',
                        body: errorMessage,
                        bodyOutputType: 'trustedHtml',
                    });
                };

                $scope.save = function()
                {
                    srvProfile
                        .put($scope.customer)
                            .success(successSave)
                                .error(failSave);
                };
            }
        }
});