angular.module("wscApp")
    .controller('ProcessCtrl', function ($scope, $location, result, srvCart, srvAccount, toaster) {

        if (!srvAccount.isUserAuthenticated()) {

            toaster.pop({
                type: 'error',
                body: "User must be logged in to checkout",
                bodyOutputType: 'trustedHtml',
            });

            $location.path("/signin");
        }

        if (result.status !== 200) {
            toaster.pop({
                type: 'error',
                body: result.data.Message,
                bodyOutputType: 'trustedHtml',
            });

            $location.path('/checkout');
        }
        else {
             srvCart.clear();
        }    $scope.processedOrder = result.data;

});