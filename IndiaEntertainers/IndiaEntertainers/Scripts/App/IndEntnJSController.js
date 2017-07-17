(function () {

    var IndEntnJSController = function ($scope, $http, $log, $window, upload) {
        $scope.ShowRegForm = true;
        $scope.loading = false;
        $scope.fielddisabler = false;
        $scope.fielddisablel = false;
        $scope.fielddisableCF = false;
        $scope.EntrId = 0;
        $scope.cntfrmrqured = true;
        $scope.ShowCntFrm = true;
        $scope.EntnMessage = {};
        $scope.CvrImg = {};
        $scope.ShowRegForm = true;
        $scope.ShowChangeCoverModel = true;
        $scope.ShowChangeProfileModel = true;
        $scope.ShowAddphtoProfileModel = true;
        $scope.ShowAddvdoProfileModel = true;
        $scope.ShowUpdateProfileModel = true;

        //Register the New User
        $scope.SubmitReqForm = function (ReqModel) {
            console.log($scope.ShowRegForm);
            $scope.ShowRegForm = false;
            console.log($scope.ShowRegForm);
            $scope.Reqmessage = "Please Wait...";
            $scope.loading = true;
            $scope.fielddisabler = true;
            $http.post('/Account/Register', ReqModel).then(function (data) {
                $scope.loading = false;
                $scope.Reqmessage = data.data;
                if ($scope.Reqmessage == "Confirmation Mail send on Your EmailId. Please confirm Email.") {
                    $scope.ShowRegForm = false;
                }
                else {
                    $scope.fielddisabler = false;
                    $scope.ShowRegForm = true;
                }
            })
                  .catch(function () {
                      $scope.error = "An Error has occured while loading posts!";
                      $scope.loading = false;
                  });
        }

        //Login to the User
        $scope.loginform = function (loginModel) {
            $scope.loginMessage = "Please Wait...";
            $scope.loading = true;
            $scope.fielddisablel = true;
            $http.post('/Account/Login', loginModel).then(function (data) {
                $scope.loading = false;
                $scope.loginMessage = data.data;
                $scope.fielddisablel = false;
                if ($scope.loginMessage == "Success") {
                    $window.location.href = "../../Home/LoginSucess";
                }
            })
             .catch(function () {
                 $scope.error = "An Error has occured while loading posts!";
                 $scope.loading = false;
             });
        }

        //Send Message to the Entertainer
        $scope.SendMessagetoEntrn = function (EntnMessage) {
            $scope.cntfrmrqured = false;
            EntnMessage.EntrId = $scope.EntrId;
            $scope.cntfrmMessage = "Please Wait...";
            $scope.loading = true;
            $scope.fielddisableCF = true;
            $http.post('/Entertainers/SendMessagetoEntrn', EntnMessage).then(function (data) {
                $scope.loading = false;
                $scope.cntfrmMessage = data.data;
                $scope.fielddisableCF = false;
                if ($scope.cntfrmMessage == "Success") {
                    $scope.cntfrmMessage = "Your Message sent Successfully!";
                    $scope.ShowCntFrm = false;
                } else {
                    $scope.cntfrmMessage = "Your Message Can't send. Please try after sometime.";
                }
            })
             .catch(function () {
                 $scope.error = "An Error has occured while loading posts!";
                 $scope.loading = false;
             });
        }

        $scope.GetEntrnId = function (id) {
            $scope.EntrId = id;
            $scope.ShowCntFrm = true;
            $scope.EntnMessage = {};
            $scope.cntfrmMessage = "";
        }

        //Chage the Cover Photo of the Entertainer
        $scope.ChangeCover = function (CoverImage) {
            console.log($scope.CvrImg);
            $scope.CoverimageMessage = "Please Wait...";
            $scope.loading = true;
            $scope.fielddisablel = true;
            $http.post('/Entertainer/MyAccount/ChangeCover', CoverImage).then(function (data) {
                $scope.loading = false;
                $scope.CoverimageMessage = data.data;
                $scope.fielddisablel = false;
                if ($scope.CoverimageMessage == "Success") {
                    // $window.location.href = "../../Home/LoginSucess";
                }
            })
             .catch(function () {
                 $scope.error = "An Error has occured while loading posts!";
                 $scope.loading = false;
             });
        }

        $scope.ChangeCoverImage = function () {
            $scope.coverImgMessage = "Please Wait, Your Photo is Uploading...";
            $scope.ShowChangeCoverModel = false;
            upload({
                url: '../../Entertainer/MyAccount/ChangeCover',
                method: 'POST',
                data: {
                    Photo: $scope.myFile,
                    Title: $scope.Title
                }
            }).then(
              function (response) {
                  $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                  console.log(response.data);
              },
              function (response) {
                  $scope.coverImgMessage = "Cant Upload Image Please try after sometime.";
                  console.error(response);
              }
            );
        }

        $scope.ChangeProfileImage = function () {
            $scope.coverPrfMessage = "Please Wait, Your Photo is Uploading...";
            $scope.ShowChangeProfileModel = false;
            upload({
                url: '../../Entertainer/MyAccount/ChangeProfileImg',
                method: 'POST',
                data: {
                    Photo: $scope.myprofilePic,
                    Title: $scope.Title
                }
            }).then(
              function (response) {
                  $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                  console.log(response.data);
              },
              function (response) {
                  $scope.coverPrfMessage = "Cant Upload Image Please try after sometime.";
                  console.error(response);
              }
            );
        }

        $scope.AddNewPhoto = function () {
            $scope.NewPhotoMessage = "Please Wait, Your Photo is Uploading...";
            $scope.ShowAddphtoProfileModel = false;
            upload({
                url: '../../Entertainer/MyAccount/AddNewPhoto',
                method: 'POST',
                data: {
                    Photo: $scope.MyNewPhoto,
                    Title: $scope.Title
                }
            }).then(
              function (response) {

                  if (response.data == "You Can add Max 8 Photos.") {
                      $scope.NewPhotoMessage = "You Can add Max 8 Photos.";
                  }
                  else if (response.data == "Success") {
                      alert("New Image Added Successfully");
                      $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                      console.log(response.data);
                  }
                  else {
                      $scope.NewPhotoMessage = "Cant Upload Image Please try after sometime.";
                      console.error(response);
                  }

              },
              function (response) {
                  $scope.NewPhotoMessage = "Cant Upload Image Please try after sometime.";
                  console.error(response);
              }
            );
        }

        $scope.DeleteImage = function (id) {
            var result = confirm('Are you sure you wish to delete this Image?');
            if (result) {
                $http.post('/Entertainer/MyAccount/DeleteImage/' + id).then(function (data) {
                    if (data.data == "Success") {
                        alert("Image Deleted Successfully!");
                        $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                    } else {
                        alert("Cant Delete Image Please try after sometime.");
                    }
                })
            .catch(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });
            }
        }

        $scope.AddVideo = function (NewWid) {
            $scope.AdVdMessage = "Please Wait..";
            $http.post('/Entertainer/MyAccount/AddVideo/', NewWid).then(function (data) {
                if (data.data == "Success") {
                    alert("Video Added Successfully!");
                    $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                } else {
                    $scope.AdVdMessage = "Can't Add Video Please try after sometime.";
                }
            })
        .catch(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
        }


        $scope.DeleteVideo = function (id) {
            var result = confirm('Are you sure you wish to delete this Video?');
            if (result) {
                $http.post('/Entertainer/MyAccount/DeleteVideo/' + id).then(function (data) {
                    if (data.data == "Success") {
                        alert("Video Deleted Successfully!");
                        $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                    } else {
                        alert("Cant Video Image Please try after sometime.");
                    }
                })
            .catch(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });
            }
        }

        $scope.GetProMessage = function () {
            $scope.UpProfileMessage = "Please Wait..";
            $scope.ShowUpdateProfileModel = false;
            $http.get('/Entertainer/MyAccount/GetMessage/').then(function (data) {
                if (data.data == "Fail") {
                    $scope.UpProfileMessage = "An Error has occured while loading posts!";
                } else {
                    $scope.UpProfileMessage = "";
                    $scope.profile = data.data;
                    $scope.ShowUpdateProfileModel = true;
                }
            })
        .catch(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
        }


        $scope.UpdateProfile = function (profile) {
            $scope.UpProfileMessage = "Please Wait..";
            $scope.ShowUpdateProfileModel = false;
            console.log(profile);
            $http.post('/Entertainer/MyAccount/UpdateProfile/?profile='+ profile).then(function (data) {
                if (data.data == "Success") {
                    alert("Update Successfully!");
                    $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                } else {
                    $scope.UpProfileMessage = data.data;
                    $scope.ShowUpdateProfileModel = true;
                }
            })
        .catch(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
        }

        $scope.GetDetails = function () {
            $scope.UpProfileMessage = "Please Wait..";
            $scope.ShowUpdateProfileModel = false;
            $http.get('/Entertainer/MyAccount/GetDetails/').then(function (data) {
                if (data.data == "Fail") {
                    $scope.UpProfileMessage = "An Error has occured while loading posts!";
                } else {
                    $scope.UpProfileMessage = "";
                    $scope.UpdDetails = data.data;
                    $scope.ShowUpdateProfileModel = true;
                }
            })
        .catch(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
        }

        $scope.UpdateProfileDetails = function (UpdDetails) {
            console.log(UpdDetails);
        }

    };

    //Add controller to the app
    app.controller("IndEntnJSController", IndEntnJSController);

}());

