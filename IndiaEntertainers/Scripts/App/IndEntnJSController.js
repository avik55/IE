(function () {
    var IndEntnJSController = function ($scope, $http, $log, $window, upload, $location, $anchorScroll) {
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
        $scope.ShowUpdateProfileDDModel = false;


        //Register the New User
        $scope.ShowRegForm = true;
        $scope.SubmitReqForm = function (ReqModel) {
            $scope.ShowRegForm = false;
            $scope.Reqmessage = [];
            $scope.Reqmessage[0] = "Please Wait...";
            $scope.loading = true;
            $scope.fielddisabler = true;
            $http.post('/Account/Register', ReqModel).then(function (data) {
                $scope.loading = false;
                $scope.Reqmessage = data.data;
                if ($scope.Reqmessage[0] == "A confirmation mail has been sent to your Email ID. Please verify the same.") {
                    $scope.Reqmessage[1] = "Please log in to your Email account to complete the Sign-up process.";
                    $scope.ShowRegForm = false;
                }
                else {
                    $scope.fielddisabler = false;
                    $scope.ShowRegForm = true;
                }
            })
                  .catch(function () {
                      $scope.error = "An Error has occured while processing your request!";
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
                    //$window.location.href = "../../Home/LoginSucess";
                    location.reload();
                }
            })
             .catch(function () {
                 $scope.error = "An Error has occured while loading posts!";
                 $scope.loading = false;
             });
        }

        //Send Message to the Entertainer
        $scope.UserLoggedIn = false;
        $scope.SendMessagetoEntrn = function (EntnMessage) {
            $scope.cntfrmrqured = false;
            EntnMessage.EntrId = $scope.EntrId;
            $scope.cntfrmMessage = "Please Wait...";
            $scope.loading = true;
            $scope.fielddisableCF = true;
            $http.post('/EntrCustom/SendMessagetoEntrn', EntnMessage).then(function (data) {
                $scope.loading = false;
                $scope.cntfrmMessage = data.data;
                $scope.fielddisableCF = false;
                var strg = $scope.cntfrmMessage.split("-");
                if (strg[0] == "Success") {
                    $scope.cntfrmMessage = "Your message has been sent to " + strg[1] + ".\nYou can also call on 8080700999 for any further query.";
                    $scope.ShowCntFrm = false;
                } else {
                    $scope.cntfrmMessage = "Unable to send mesages currently.\n Please try again after some time or for immediate assistance, you can call IndiaEntertainers.com support.";
                }
            })
             .catch(function () {
                 $scope.error = "An Error has occured while processing your request!";
                 $scope.loading = false;
             });
        }

        $scope.GetEntrnId = function (id) {
            $scope.EntrId = id;
            $http.get('/EntrCustom/ChckUserIsLogin').then(function (data) {
                $scope.chcksms = data.data;
                if ($scope.chcksms == "true") {
                    $scope.UserLoggedIn = true;
                    $scope.EntnMessage.Name = "-";
                    $scope.EntnMessage.EmailID = "email@email.com";
                    $scope.EntnMessage.ContactNo = "-";
                } else {
                    $scope.UserLoggedIn = false;
                }
            })
            .catch(function () {
                $scope.error = "An Error has occured while processing your request!";
                $scope.loading = false;
            });
            $scope.ShowCntFrm = true;
            $scope.EntnMessage = {};
            $scope.cntfrmMessage = "";
        }

        //Chage the Cover Photo of the Entertainer
        $scope.ChangeCover = function (CoverImage) {
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
                 $scope.error = "An Error has occured while processing your request!";
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
              },
              function (response) {
                  $scope.coverImgMessage = "Cant Upload Image Please try after sometime.";
              }
            );
        }

        $scope.ChangeProfileImage = function () {
            debugger;
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
              },
              function (response) {
                  $scope.coverPrfMessage = "Cant Upload Image Please try after sometime.";
              }
            );
        }

        $scope.AddProfilePicRegister = function () {
            upload({
                url: '/Account/ChangeProfileImg',
                method: 'POST',
                data: {
                    Photo: $scope.myprofilePic,
                    Title: $scope.Title
                }
            }).then(function (response) { }, function (response) { });
        }

        $scope.AddCoverPicRegister = function () {
            upload({
                url: '/Account/ChangeCover',
                method: 'POST',
                data: {
                    Photo: $scope.myFile,
                    Title: $scope.Title
                }
            }).then(
              function (response) { }, function (response) { }
            );
        }

        $scope.getFileDetails = function (e) {
            $scope.files = [];
            $scope.$apply(function () {
                for (var i = 0; i < e.files.length; i++) {
                    $scope.files.push(e.files[i])
                }
            });
        };

        //$scope.getFilePreview = function (e) {
        //    debugger;
        //    $scope.files = [];
        //    if (e.files.length <= 25) {
        //        $scope.$apply(function () {
        //            for (var i = 0; i < e.files.length; i++) {
        //                $scope.files.push(e.files[i])
        //            }
        //            var filesAmount = e.files.length;
        //            var counter = 0;
        //            for (i = 0; i < filesAmount; i++) {
        //                var reader = new FileReader();
        //                reader.onload = function (event) {
        //                    debugger;
        //                    var ImagePreviewHtml = "<div class='col-md-6 imageDiv' style='width: 20%;'>"
        //                                         + "<div class='trending-wrap' style='height: 150px;width: 150px;'>"
        //                                         + "<div class='trend_img' style='position:relative;'>"
        //                                         + "<a style='color:black;' onClick='yourFunction(this);' href='javascript:;'>del</a>"
        //                                         + "<a>"
        //                                         + "<img src='" + event.target.result + "' id='img" + counter + "' style='height: 150px;width: 150px;padding: 4px;'/>"
        //                                         + "</a>"
        //                                         + "</div>"
        //                                         + "</div>"
        //                                         + "</div>";
        //                    $("#formprofileCompleteDetails").find('div[id=gallery]').append(ImagePreviewHtml);
        //                }
        //                reader.readAsDataURL(e.files[i]);
        //            }
        //        });
        //        $scope.AddNewPhotoRegister();
        //    } else {
        //        alert("Maximum 25 photos are allowed to upload.")
        //    }
        //};

        $scope.getFilePreview = function (e) {
            $scope.files = [];
            if (e.files.length <= 25) {
                $scope.$apply(function () {
                    for (var i = 0; i < e.files.length; i++) {
                        $scope.files.push(e.files[i])
                    }

                    var files = e.files;
                    for (var i = 0, f; f = files[i]; i++) {

                        if (!f.type.match('image.*')) { continue; }

                        var reader = new FileReader();
                        reader.onload = (function (theFile) {
                            return function (e) {
                                var ImagePreviewHtml = "<div class='col-md-6 imageDiv' style='width: 20%;'>"
                                                         + "<div class='trending-wrap' style='height: 150px;width: 150px;'>"
                                                         + "<div class='trend_img' style='position:relative;'>"
                                                         + "<span class='text-info text-left' style='position:absolute; padding:10px;left:0;'>"
                                                         + "<a style='color: white;' onClick='deleteImgFunction(this);' data-title='" + theFile.name + "' href='javascript:;'>"
                                                         + "<i class='fa fa-trash-o' aria-hidden='true' style='font-size: large;'></i>"
                                                         + "</a>"
                                                         + "</span>"
                                                         + "<a>"
                                                         + "<img src='" + e.target.result + "' style='height: 150px;width: 150px;padding: 4px;'/>"
                                                         + "</a>"
                                                         + "</div>"
                                                         + "</div>"
                                                         + "</div>";

                                $("#formprofileCompleteDetails").find('div[id=gallery]').append(ImagePreviewHtml);
                            };
                        })(f);
                        reader.readAsDataURL(f);
                    }
                });
                $scope.AddNewPhotoRegister();
            } else {
                alert("Maximum 25 photos are allowed to upload.")
            }
        };

        $scope.AddNewPhotoRegister = function () {
            $("#formprofileCompleteDetails").find('img[class=imgLoaderMultipleImg]').show();
            var counter = 0;
            if ($scope.files.length <= 25) {
                for (var i = 0; i < $scope.files.length; i++) {
                    var file = $scope.files[i];
                    upload({
                        url: '/Account/AddNewPhoto',
                        method: 'POST',
                        data: {
                            Photo: file, Title: file.name
                        }
                    }).then(function (response) {
                        if (response.data == "Success") {
                            if (counter == 0) {
                                counter = 1;
                            }
                        }
                        if (counter == 1) {
                            setTimeout(function () {
                                alert("Pictures uploaded successfully.");
                                $("#formprofileCompleteDetails").find('img[class=imgLoaderMultipleImg]').hide();
                            }, 1000);
                        }
                        counter++;
                    });
                }
            } else {
                alert("You Can add maximum 25 photos.");
                $("#formprofileCompleteDetails").find('img[class=imgLoaderMultipleImg]').hide();
            }
        }

        $scope.AddNewPhoto = function () {
            $scope.NewPhotoMessage = "Please Wait, Your Photo is Uploading...";
            $scope.ShowAddphtoProfileModel = false;

            if ($scope.files.length <= 8) {
                for (var i = 0; i < $scope.files.length; i++) {
                    var file = $scope.files[i];
                    upload({
                        url: '../../Entertainer/MyAccount/AddNewPhoto',
                        method: 'POST',
                        data: {
                            Photo: file, Title: $scope.Title
                        }
                    }).then(function (response) {

                        if (response.data == "You Can add Max 8 Photos.") {
                            $scope.NewPhotoMessage = "You Can add Max 8 Photos.";
                        }
                        else if (response.data == "Success") { $window.location.href = "../../Entertainer/MyAccount/ProfileDetail"; }
                        else { $scope.NewPhotoMessage = "Cant Upload Image Please try after sometime."; }
                    }, function (response) { $scope.NewPhotoMessage = "Cant Upload Image Please try after sometime."; });
                }
            } else {
                $scope.NewPhotoMessage = "You Can add Max 8 Photos.";
            }
        }

        $scope.CloseAddNewPhoto = function () {
            $scope.ShowAddphtoProfileModel = true;
            $scope.NewPhotoMessage = "";
        }

        $scope.DeleteImage = function (id) {
            var result = confirm('Are you sure you wish to delete this Image?');
            if (result) {
                $http.post('/Entertainer/MyAccount/DeleteImage/' + id).then(function (data) {
                    if (data.data == "Success") {
                        // alert("Image Deleted Successful!");
                        $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                    } else {
                        alert("Cant Delete Image Please try after sometime.");
                    }
                })
            .catch(function () {
                $scope.error = "An Error has occured while processing your request!";
                $scope.loading = false;
            });
            }
        }

        $scope.AddVideo = function (NewWid) {
            $scope.AdVdMessage = "Please Wait..";
            $http.post('/Entertainer/MyAccount/AddVideo/', NewWid).then(function (data) {
                if (data.data == "Success") {
                    //alert("Video Added Successful!");
                    $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                } else {
                    $scope.AdVdMessage = "Can't Add Video Please try after sometime.";
                }
            })
        .catch(function () {
            $scope.error = "An Error has occured while processing your request!";
            $scope.loading = false;
        });
        }

        $scope.AddVideoNew = function (NewWid) {

            var Videolink1 = $("#AddVideoNew").find('input[name=videoLinktest1]').val();
            var VideoLink2 = $("#AddVideoNew").find('input[name=videoLinktest2]').val();
            var VideoLink3 = $("#AddVideoNew").find('input[name=videoLinktest3]').val();
            var VideoLink4 = $("#AddVideoNew").find('input[name=videoLinktest4]').val();
            var VideoLink5 = $("#AddVideoNew").find('input[name=videoLinktest5]').val();

            if (Videolink1) {
                if (Videolink1 != "") {
                    $scope.AddVideoNewFunction(Videolink1);
                }
            }

            if (VideoLink2) {
                if (VideoLink2 != "") {
                    $scope.AddVideoNewFunction(VideoLink2);
                }
            }

            if (VideoLink3) {
                if (VideoLink3 != "") {
                    $scope.AddVideoNewFunction(VideoLink3);
                }
            }

            if (VideoLink4) {
                if (VideoLink4 != "") {
                    $scope.AddVideoNewFunction(VideoLink4);
                }
            }

            if (VideoLink5) {
                if (VideoLink5 != "") {
                    $scope.AddVideoNewFunction(VideoLink5);
                }
            }

            setTimeout(function () {
                $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
            }, 1000);



            //    $scope.AdVdMessage = "Please Wait..";
            //    $http.post('/Entertainer/MyAccount/AddVideo/', NewWid).then(function (data) {
            //        if (data.data == "Success") {
            //            //alert("Video Added Successful!");
            //            $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
            //        } else {
            //            $scope.AdVdMessage = "Can't Add Video Please try after sometime.";
            //        }
            //    })
            //.catch(function () {
            //    $scope.error = "An Error has occured while processing your request!";
            //    $scope.loading = false;
            //});
        }

        $scope.AddVideoNewFunction = function (videoLink) {
            debugger;

            var videoFileLink = {
                Videolink: videoLink,
                Title: ""
            };

            $http.post('/Entertainer/MyAccount/AddVideo/', videoFileLink).then(function (data) {
                if (data.data == "Success") {
                    //alert("Video Added Successful!");
                    //$window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                } else {
                    $scope.AdVdMessage = "Can't Add Video Please try after sometime.";
                }
            })
            .catch(function () {
                $scope.error = "An Error has occured while processing your request!";
                $scope.loading = false;
            });


        }

        $scope.AddVideoRegister = function (NewWid) {
            $("#formprofileCompleteDetails").find('img[class=imgLoaderMultipleVid]').show();
            $scope.AdVdMessage = "Please Wait..";
            debugger;
            var Videolink1 = $("#AddNewVideoRegister").find('input[name=videoLinktest1]').val();
            var VideoLink2 = $("#AddNewVideoRegister").find('input[name=videoLinktest2]').val();
            var VideoLink3 = $("#AddNewVideoRegister").find('input[name=videoLinktest3]').val();
            var VideoLink4 = $("#AddNewVideoRegister").find('input[name=videoLinktest4]').val();
            var VideoLink5 = $("#AddNewVideoRegister").find('input[name=videoLinktest5]').val();

            if (Videolink1) {
                if (Videolink1 != "") {
                    $scope.AddVideosFunction(Videolink1);
                }
            }

            if (VideoLink2) {
                if (VideoLink2 != "") {
                    $scope.AddVideosFunction(VideoLink2);
                }
            }

            if (VideoLink3) {
                if (VideoLink3 != "") {
                    $scope.AddVideosFunction(VideoLink3);
                }
            }

            if (VideoLink4) {
                if (VideoLink4 != "") {
                    $scope.AddVideosFunction(VideoLink4);
                }
            }

            if (VideoLink5) {
                if (VideoLink5 != "") {
                    $scope.AddVideosFunction(VideoLink5);
                }
            }
            $scope.AdVdMessage = "";
            $("#AddNewVideoRegister").find('button[id=closeVideoPopUp]').trigger('click');
            $("#formprofileCompleteDetails").find('img[class=imgLoaderMultipleVid]').hide();

        }

        $scope.AddVideosFunction = function (videoLink) {
            debugger;  
            var title = videoLink.substr(videoLink.indexOf("=") + 1)
            title = title.toLowerCase();
           
            var videoFileLink = {
                Videolink: videoLink,
                Title: title
            };
            //NewWid.Videolink = videoLink;
            //NewWid.Title = "";
            $http.post('/Account/AddVideo/', videoFileLink).then(function (data) {
                if (data.data == "Fail") {
                    alert("Can't Add Video Please try after sometime.");
                } else {
                    var VideoImgPreviewHtml = "<div class='col-md-6 VideoDiv' style='width: 20%;'>"
                                             + "<div class='trending-wrap' style='width: 150px;'>"
                                             + "<div class='trend_img' style='position:relative;'>"
                                             + "<span class='text-info text-left' style='position:absolute; padding:10px;left:0;'>"
                                             + "<a href='javascript:;' data-vidtitle='" + title + "' onClick='deleteVideoFunction(this);' title='Delete Video' style='color:white;'>"
                                             + "<i class='fa fa-trash-o' aria-hidden='true' style='font-size: large;'></i>"
                                             + "</a>"
                                             + "</span>"
                                             + "<span class='video-icon'></span>"
                                             + "<img src='" + data.data + "' class='img-responsive video-cover-img'/>"                                             
                                             + "</div>"
                                             + "</div>"
                                             + "</div>";

                    $("#formprofileCompleteDetails").find('div[id=galleryVideo]').append(VideoImgPreviewHtml);
                    $("#AddNewVideoRegister").find('input[name=videoLinktest1]').val("");
                    $("#AddNewVideoRegister").find('input[name=videoLinktest2]').val("");
                    $("#AddNewVideoRegister").find('input[name=videoLinktest3]').val("");
                    $("#AddNewVideoRegister").find('input[name=videoLinktest4]').val("");
                    $("#AddNewVideoRegister").find('input[name=videoLinktest5]').val("");
                }
            }).catch(function () {
                alert("Exception");
            });
        }

        $scope.DeleteVideo = function (id) {
            var result = confirm('Are you sure you wish to delete this Video?');
            if (result) {
                $http.post('/Entertainer/MyAccount/DeleteVideo/' + id).then(function (data) {
                    if (data.data == "Success") {
                        //  alert("Video Deleted Successful!");
                        $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                    } else {
                        alert("Cant Video Image Please try after sometime.");
                    }
                })
            .catch(function () {
                $scope.error = "An Error has occured while processing your request!";
                $scope.loading = false;
            });
            }
        }

        $scope.GetProMessage = function () {
            $scope.UpProfileMessage = "Please Wait..";
            $scope.ShowUpdateProfileModel = false;
            $http.get('/Entertainer/MyAccount/GetMessage/').then(function (data) {
                if (data.data == "Fail") {
                    $scope.UpProfileMessage = "An Error has occured while processing your request!";
                } else {
                    $scope.Proppp = {};
                    $scope.Proppp.ProfTitle = data.data;
                    $scope.UpProfileMessage = "";
                    $scope.profile = data.data;
                    $scope.ShowUpdateProfileModel = true;
                }
            })
        .catch(function () {
            $scope.error = "An Error has occured while processing your request!";
            $scope.loading = false;
        });
        }

        $scope.UpdateProfile = function () {
            $scope.UpProfileMessage = "Please Wait..";
            $scope.ShowUpdateProfileModel = false;
            $http.post('/Entertainer/MyAccount/UpdateProfile', $scope.Proppp).then(function (data) {
                if (data.data == "Success") {
                    alert("Updated Successfully");
                    $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                } else {
                    $scope.UpProfileMessage = data.data;
                    $scope.ShowUpdateProfileModel = true;
                }
            })
        .catch(function () {
            $scope.error = "An Error has occured while processing your request!";
            $scope.loading = false;
        });
        }

        //
        //GetCities
        $scope.CityyTitle = "-Select City-";
        $scope.LoadEditFormShow = false;
        $scope.UpdDetails = {};
        $scope.AllCities = [];
        $scope.CatTitle = "-Select Category-";
        $scope.UpdDetails.PerfLanges = [];
        $scope.Cat1Cheked = false;
        $scope.Cat2Cheked = false;
        $scope.Cat3Cheked = false;
        $scope.Cat4Cheked = false;
        $scope.Cat5Cheked = false;
        $scope.Cat6Cheked = false;
        $scope.Cat7Cheked = false;
        $scope.Noshw1Cheked = false;
        $scope.Noshw2Cheked = false;
        $scope.Noshw3Cheked = false;
        $scope.Noshw4Cheked = false;
        $scope.PerfLength1Cheked = false;
        $scope.PerfLength2Cheked = false;
        $scope.PerfLength3Cheked = false;
        $scope.PerformanceLanguages = "";
        $scope.PerfLang1Cheked = false;
        $scope.PerfLang2Cheked = false;
        $scope.PerfLang3Cheked = false;

        $scope.GetDetails = function () {
            debugger;
            $scope.UpProfileMessage = "Please Wait..";
            $scope.ShowUpdateProfileModel = false;
            $http.get('/Entertainer/MyAccount/GetDetails/').then(function (data) {
                if (data.data == "Fail") {
                    $scope.UpProfileMessage = "An Error has occured while processing your request!";
                } else {
                    $scope.UpProfileMessage = "";
                    $scope.UpdDetails = data.data;
                    $scope.CityyTitle = $scope.UpdDetails.City;
                    $scope.CatTitle = "";
                    if ($scope.UpdDetails.CatId == 15) {
                        $scope.CatTitle = "Bands"; $scope.Cat1Cheked = true;
                    } else if ($scope.UpdDetails.CatId == 9) {
                        $scope.CatTitle = "Dancers"; $scope.Cat2Cheked = true;
                    } else if ($scope.UpdDetails.CatId == 11) {
                        $scope.CatTitle = "DJs"; $scope.Cat3Cheked = true;
                    } else if ($scope.UpdDetails.CatId == 5) {
                        $scope.CatTitle = "Anchors"; $scope.Cat4Cheked = true;
                    } else if ($scope.UpdDetails.CatId == 2) {
                        $scope.CatTitle = "Singers"; $scope.Cat5Cheked = true;
                    } else if ($scope.UpdDetails.CatId == 12) {
                        $scope.CatTitle = "Stand-up Comedians"; $scope.Cat6Cheked = true;
                    } else if ($scope.UpdDetails.CatId == 14) {
                        $scope.CatTitle = "Uncut (Aspiring Singers)"; $scope.Cat7Cheked = true;
                    }

                    $scope.NoOfShow = 0;
                    if ($scope.UpdDetails.NoOfShow == 50) {
                        $scope.NoOfShow = 50; $scope.Noshw1Cheked = true;
                    } else if ($scope.UpdDetails.NoOfShow == 100) {
                        $scope.NoOfShow = 100; $scope.Noshw2Cheked = true;
                    } else if ($scope.UpdDetails.NoOfShow == 200) {
                        $scope.NoOfShow = 200; $scope.Noshw3Cheked = true;
                    } else if ($scope.UpdDetails.NoOfShow == 500) {
                        $scope.NoOfShow = 500; $scope.Noshw4Cheked = true;
                    } else if ($scope.UpdDetails.NoOfShow == 1000) {
                        $scope.NoOfShow = 1000; $scope.Noshw5Cheked = true;
                    } else if ($scope.UpdDetails.NoOfShow == 2000) {
                        $scope.NoOfShow = 2000; $scope.Noshw6Cheked = true;
                    }

                    $scope.PerfLength = 0;
                    if ($scope.UpdDetails.PerfLength == 30) {
                        $scope.PerfLength = 30; $scope.PerfLength1Cheked = true;
                    } else if ($scope.UpdDetails.PerfLength == 60) {
                        $scope.PerfLength = 60; $scope.PerfLength2Cheked = true;
                    } else if ($scope.UpdDetails.PerfLength == 120) {
                        $scope.PerfLength = 120; $scope.PerfLength3Cheked = true;
                    }

                    if ($scope.UpdDetails.PerfLang == "English") {
                        $scope.PerfLang1Cheked = true;
                    }
                    if ($scope.UpdDetails.PerfLang1 == "Hindi") {
                        $scope.PerfLang2Cheked = true;
                    }
                    if ($scope.UpdDetails.PerfLang2 == "Multilingual") {
                        $scope.PerfLang3Cheked = true;
                    }
                    // $scope.PerformanceLanguages = $scope.UpdDetails.PerfLangTitle;
                    $scope.ShowUpdateProfileModel = true;
                }
            })
        .catch(function () {
            $scope.error = "An Error has occured while processing your request!";
            $scope.loading = false;
        });
        }

        //
        $scope.UpdateProfileDetails = function (UpdDetails) {
            debugger;
            if (UpdDetails.Showfee == true) {
                UpdDetails.DonotShowfee = false;
            } else {
                UpdDetails.DonotShowfee = true;
            }

            $scope.UpProfileUpdMessage = "Please Wait..";
            $scope.ShowUpdateProfileDDModel = false;
            $http.post('/Entertainer/MyAccount/UpdateProfileDetails', $scope.UpdDetails).then(function (data) {
                if (data.data == "Success") {
                    alert("Updated Successfully");
                    $window.location.href = "../../Entertainer/MyAccount/ProfileDetail";
                } else {
                    $scope.UpProfileUpdMessage = data.data;
                    $scope.ShowUpdateProfileDDModel = true;
                }
            })
        .catch(function () {
            $scope.UpProfileUpdMessage = "An Error has occured while processing your request!";
            $scope.loading = false;
        });
        }

        //Multi Languages
        $scope.ChangeLangTitle = function (lang, name) {
            if (name == "E") {
                if (lang == true) {
                    $scope.PerfLang1Cheked = true;
                    $scope.PerfLang = true;
                } else {
                    $scope.PerfLang1Cheked = false;
                    $scope.PerfLang = false;
                }

            } else if (name == "H") {
                if (lang == true) {
                    $scope.PerfLang2Cheked = true;
                    $scope.PerfLang1 = true;
                } else {
                    $scope.PerfLang2Cheked = false;
                    $scope.PerfLang1 = false;
                }
            }
            else if (name == "R") {
                if (lang == true) {
                    $scope.PerfLang3Cheked = true;
                    $scope.PerfLang2 = true;
                } else {
                    $scope.PerfLang3Cheked = false;
                    $scope.PerfLang2 = false;
                }
            }
        }

        $scope.SetCatTitle = function (Id) {
            if (Id == 15) {
                $scope.CatTitle = "Bands";
            } else if (Id == 9) {
                $scope.CatTitle = "Dancers";
            } else if (Id == 11) {
                $scope.CatTitle = "DJs";
            } else if (Id == 5) {
                $scope.CatTitle = "Anchors";
            } else if (Id == 2) {
                $scope.CatTitle = "Singers";
            } else if (Id == 12) {
                $scope.CatTitle = "Stand-up Comedians";
            } else if (Id == 14) {
                $scope.CatTitle = "Uncut (Aspiring Singers)";
            }
        }

        $scope.SetCityTitle = function (id) {
            $scope.CityyTitle = id;
            $scope.ShowCitiesFltrModel = false;
        }

        //GetCities
        $scope.ShowCitiesFltrModel = false;
        $scope.GetCities = function (Qury) {
            $scope.SrchFiltcityMessage = "Please Wait..";
            if (Qury != null) {
                if (Qury.length >= 2) {
                    $http.get('/EntrCustom/GetCities?Qry=' + Qury).then(function (data) {
                        if (data.data == "Fail") {
                            $scope.ShowCitiesFltrModel = false;
                            $scope.SrchFiltcityMessage = "An Error has occured while processing your request!";
                        } else {
                            $scope.SrchFiltcityMessage = "";
                            $scope.AllCities = data.data;
                            $scope.ShowCitiesFltrModel = true;
                        }
                    })
            .catch(function () {
                $scope.error = "An Error has occured while processing your request!";
                $scope.loading = false;
            });
                }
                else {
                    $scope.ShowCitiesFltrModel = false;
                    UpdDetails.CityId = null;
                }
            }
        }

        //Change Password
        $scope.ShowChngPassForm = true;
        $scope.ChangePassword = function (ChngPassModel) {
            $scope.ShowChngPassForm = false;
            $scope.Chngpassmsg = [];
            $scope.Chngpassmsg[0] = "Please Wait...";
            $http.post('http://www.indiaentertainers.com/Account/ChangePassword', $scope.ChngPassModel).then(function (data) {
                $scope.Chngpassmsg = data.data;
                $scope.ShowChngPassForm = true;
                if ($scope.Chngpassmsg[0] == "Your password change Successful!") {
                    $scope.ShowChngPassForm = true;
                    alert("Your password changed Successfully!");
                    $window.location.href = "http://www.indiaentertainers.com";
                }
                else {
                    $scope.ShowChngPassForm = true;
                }
            })
             .catch(function () {
                 $scope.UpProfileUpdMessage = "An Error has occured while processing your request!";
                 $scope.loading = false;
             });
        }

        //Forgot Password
        $scope.ShowfrgotPassForm = true;
        $scope.Hideloginform = function () {
            debugger;
            if (this.loginModel) {
                var username = this.loginModel.UserName;
                $("#forgotpasswordmodal").find("input[name=UserName]").val(username);
            }
            $('#myModal').removeClass('modal');
        }

        $scope.ForgotPassword = function () {
            $scope.ShowfrgotPassForm = false;
            $scope.forgotpassmsg = [];
            $scope.forgotpassmsg[0] = "Please Wait...";
            $http.post('../../Account/ForgotPassword?UserEmail=' + $scope.UserEmail + '&UserName=' + $scope.UserName).then(function (data) {
                $scope.forgotpassmsg = data.data;
                $scope.ShowChngPassForm = true;
                if ($scope.forgotpassmsg[0] == "Email sent on your mail id to reset password.") {
                    $scope.ShowfrgotPassForm = true;
                    alert("Email sent on your mail id to reset password.");
                    $window.location.href = "../Home/Index";
                }
                else {
                    $scope.ShowfrgotPassForm = true;
                }
            })
             .catch(function () {
                 $scope.UpProfileUpdMessage = "An Error has occured while processing your request!";
                 $scope.loading = false;
             });
        }

        //Contact to IE
        //Send Message to the IE
        $scope.UserLoggedInCTIE = false;
        $scope.ShowCntIEFrm = true;

        $scope.ChckLogin = function () {
            $http.get('/EntrCustom/ChckUserIsLogin').then(function (data) {
                $scope.chcksms = data.data;
                if ($scope.chcksms == "true") {
                    $scope.UserLoggedInCTIE = true;
                    $scope.IEMessage.Name = "-";
                    $scope.IEMessage.EmailID = "email@email.com";
                    $scope.IEMessage.ContactNo = "-";
                } else {
                    $scope.UserLoggedInCTIE = false;
                }
            })
            .catch(function () {
                $scope.error = "An Error has occured while processing your request!";
                $scope.loading = false;
            });
        }

        $scope.SendMessagetoIE = function (IEMessage) {
            IEMessage.EntrId = 0;
            $scope.cntfrmIEMessage = "Please Wait...";
            $scope.loading = true;
            $http.post('/Home/SendMessagetoIE', IEMessage).then(function (data) {
                $scope.loading = false;
                $scope.cntfrmIEMessage = data.data;
                if ($scope.cntfrmIEMessage == "Success") {
                    //$scope.cntfrmIEMessage = "Thank you for contacting us. We will revert to you at the earliest.";
                    $scope.cntfrmIEMessage = "Your message has reached us. We will get back to you at the earliest";
                    $scope.ShowCntIEFrm = false;
                } else {
                    $scope.cntfrmIEMessage = "Unable to send mesages currently. Please try again after some time or for immediate assistance, you can call IndiaEntertainers.com support.";
                }
            })
             .catch(function () {
                 $scope.error = "An Error has occured while processing your request!";
                 $scope.loading = false;
             });
        }

        //Subscribe New User
        $scope.SubTextDisable = false;
        $scope.SubscribeUser = function (Email) {
            $scope.SubTextDisable = true;
            $scope.submsg = "Please Wait...";
            $("#Subscribeform img[class=imgLoaderSubscribe]").show();
            $http.post('/Home/Subscribe?EmailId=' + Email).then(function (data) {
                $scope.loading = false;
                $scope.cntfrmIEMessage = data.data;
                if ($scope.cntfrmIEMessage == "Success") {
                    $("#Subscribeform img[class=imgLoaderSubscribe]").hide();
                    alert("Thank you for subscribing to our newsletter.\nWe will endeavour to keep you updated with 'the best in Entertainment'.");
                    $scope.submsg = "";
                    $scope.SubEmail = "";
                    $scope.SubTextDisable = false;
                }
                else if ($scope.cntfrmIEMessage == "Duplicate") {
                    //alert("Your EmailId Already Subscribed.");
                    $("#Subscribeform img[class=imgLoaderSubscribe]").hide();
                    alert("This Email ID has already been registered to receive the newsletters.\nIn case of any assistance, please call on 8080700999.");
                    $scope.submsg = "";
                    $scope.SubEmail = "";
                    $scope.SubTextDisable = false;
                }
                else {
                    $("#Subscribeform img[class=imgLoaderSubscribe]").hide();
                    alert("An Error has occured while processing your request!");
                    $scope.ShowCntIEFrm = "Your Message Can't send. Please try after sometime.";
                }
            })
            .catch(function () {
                $("#Subscribeform img[class=imgLoaderSubscribe]").hide();
                $scope.error = "An Error has occured while processing your request!";
                $scope.loading = false;
            });
        }

        ///Load Entertainers on HomePage
        $scope.GetEntertaines = function () {
            $http.get('/Home/GetEntertaines').then(function (data) {
                $scope.TrendingEntertainers = data.data;
            })
        .catch(function () {
            $scope.error = "An Error has occured while processing your request!";
            $scope.loading = false;
        });
        }

        $scope.submitRegForm = function (valid, RegistrationModel) {
            if (valid) {
                $("#frmRegistration").find('img[class=imgLoader]').show();
                $("#formprofileCompleteDetails").find('input[name=FName]').val(RegistrationModel.fname);
                $("#formprofileCompleteDetails").find('input[name=CityID]').val(RegistrationModel.city);
                $("#formprofileCompleteDetails").find('input[name=Mobile]').val(RegistrationModel.Mobile);
                $http.post('/Account/Register', RegistrationModel).then(function (data) {
                    $("#frmRegistration").find('img[class=imgLoader]').hide();
                    var Reqmessage = data.data;
                    if (Reqmessage) {
                        $("#frmOTPvalidation").find('input[name=hdnOTP]').val(Reqmessage[0]);
                        $("#frmOTPvalidation").find('input[name=hdnUserID]').val(Reqmessage[1]);
                    }
                }).catch(function () {
                    $("#frmRegistration").find('img[class=imgLoader]').hide();
                });
                $("#frmRegistration").hide();
            }
        }

        $scope.submitTalentSeekarRegForm = function (valid, RegistrationModel) {
            if (valid) {
                $("#frmTalentSeekarRegistration").find('img[class=imgLoader]').show();

                $("#frmTalentSeekarOTPvalidation").find('input[name=hdnfname]').val(RegistrationModel.fname);
                $("#frmTalentSeekarOTPvalidation").find('input[name=hdnlname]').val(RegistrationModel.lname);
                $("#frmTalentSeekarOTPvalidation").find('input[name=hdncompanyname]').val(RegistrationModel.companyname);
                $("#frmTalentSeekarOTPvalidation").find('input[name=hdnCityId]').val(RegistrationModel.city);
                $("#frmTalentSeekarOTPvalidation").find('input[name=hdnemail]').val(RegistrationModel.Email);

                $http.post('/Account/Register', RegistrationModel).then(function (data) {
                    $("#frmTalentSeekarRegistration").find('img[class=imgLoader]').hide();
                    var Reqmessage = data.data;
                    if (Reqmessage) {
                        $("#frmTalentSeekarOTPvalidation").find('input[name=hdnOTP]').val(Reqmessage[0]);
                        $("#frmTalentSeekarOTPvalidation").find('input[name=hdnUserID]').val(Reqmessage[1]);
                        $("#frmTalentSeekarOTPvalidation").find('input[name=hdnSentMail]').val(Reqmessage[2]);
                    }
                }).catch(function () {
                    $("#frmTalentSeekarRegistration").find('img[class=imgLoader]').hide();
                });
                //$("#frmTalentSeekarRegistration").hide();
            }
        }

        $scope.submitTalentSeekarOTPForm = function (valid) {
            if (valid) {
                var userId = $("#frmTalentSeekarOTPvalidation").find('input[name=hdnUserID]').val();
                var firstname = $("#frmTalentSeekarOTPvalidation").find('input[name=hdnfname]').val();
                var lastname = $("#frmTalentSeekarOTPvalidation").find('input[name=hdnlname]').val();
                var companyname = $("#frmTalentSeekarOTPvalidation").find('input[name=hdncompanyname]').val();
                var email = $("#frmTalentSeekarOTPvalidation").find('input[name=hdnemail]').val();
                var cityid = $("#frmTalentSeekarOTPvalidation").find('input[name=hdnCityId]').val();
                $("#frmTalentSeekarOTPvalidation").find('img[class=imgLoader]').show();
                debugger;
                $.post("/Account/ConfirmOTPTalentSeekar", {
                    userId: userId,
                    firstname: firstname,
                    lastname: lastname,
                    companyname: companyname,
                    email: email,
                    cityid: cityid
                }, function (result) {
                    debugger;
                    if (result) {
                        $("#frmTalentSeekarOTPvalidation").find('img[class=imgLoader]').hide();
                        if (result == "Success") {
                            $("#frmTalentSeekarOTPvalidation").find('a[id=openSuccessMsgTalentSeeker]').trigger('click');
                        }
                    }
                });

            }
        }

        $scope.submitOTPForm = function (valid) {
            if (valid) {
                var userId = $("#frmOTPvalidation").find('input[name=hdnUserID]').val();
                $("#frmOTPvalidation").find('img[class=imgLoader]').show();
                $.post("/Account/ConfirmOTP", { userId: userId }, function (result) {
                    if (result) {
                        $("#frmOTPvalidation").find('img[class=imgLoader]').hide();
                        if (result != "Success" && result != "Error") {
                            $("#formprofileCompleteDetails").find('input[name=EntrId]').val(result);
                        }
                    }
                });
                $("#frmOTPvalidation").hide();
            }
        }

        $scope.submitAddCategoryForm = function (valid, CategoryModel) {
            debugger;
            if (valid) {

                if (CategoryModel.selected) {
                    if (CategoryModel.selected == true) {
                        CategoryModel.Showfee = false;
                    }

                    if (CategoryModel.selected == false) {
                        CategoryModel.Showfee = true;
                    }
                } else {
                    CategoryModel.Showfee = true;
                }

                $("#frmAddCategory").find('img[class=imgLoader]').show();
                $("#formprofileCompleteDetails").find('input[name=CatId]').val(CategoryModel.CatId);
                $("#formprofileCompleteDetails").find('input[name=Showfee]').val(CategoryModel.Showfee);
                $("#formprofileCompleteDetails").find('input[name=Performancefee]').val(CategoryModel.Performancefee);
                $("#frmAddCategory").find('img[class=imgLoader]').hide();
                $("#frmAddCategory").hide();
            }
        }

        $scope.submitProfileDetailsForm = function (valid, profileDetailModel) {
            if (valid) {
                $("#formProfileDetails").find('img[class=imgLoader]').show();
                $("#formprofileCompleteDetails").find('input[name=PerfLength]').val(profileDetailModel.PerfLength);
                $("#formprofileCompleteDetails").find('input[name=YearsOfPerforming]').val(profileDetailModel.YearsOfPerforming);
                $("#formprofileCompleteDetails").find('input[name=PerfMembers]').val(profileDetailModel.PerfMembers);
                $("#formprofileCompleteDetails").find('input[name=NoofShow]').val(profileDetailModel.NoofShow);
                $("#formprofileCompleteDetails").find('input[name=PerfLanguage]').val(profileDetailModel.PerfLanguage);
                $("#formprofileCompleteDetails").find('input[name=Gender]').val(profileDetailModel.Gender);
                $("#formprofileCompleteDetails").find('input[name=Nationality]').val(profileDetailModel.Nationality);
                $("#formProfileDetails").find('img[class=imgLoader]').hide();
            }
        }

        $scope.submitProfileCompleteDetail = function (valid, profileCompleteDetailModel) {
            debugger;
            var profileTxt = $("#formprofileCompleteDetails").find('textarea[id=profiletitle]').val()
            var profileDetailTxt = $("#formprofileCompleteDetails").find('textarea[id=detailprofileid]').val()
            if (!valid) {
                if (profileTxt == "" || profileTxt.length < 20 || profileDetailTxt == "" || profileDetailTxt.length < 150) {
                    $location.hash('profileTxtDiv');
                    $anchorScroll();
                }
            }

            var profileImg = $("#formprofileCompleteDetails").find('input[id=myprofilePic]').val();
            var CoverImg = $("#formprofileCompleteDetails").find('input[id=myFile]').val();

            if (profileImg == '') {
                $("#formprofileCompleteDetails").find('p[id=profilePicError]').show();
                return false;
            }

            if (CoverImg == '') {
                $("#formprofileCompleteDetails").find('p[id=coverPicError]').show();
                return false;
            }

            if (valid) {
                $("#formprofileCompleteDetails").find('img[class=imgLoader]').show();
                profileCompleteDetailModel.FName = $("#formprofileCompleteDetails").find('input[name=FName]').val();
                profileCompleteDetailModel.EntrId = $("#formprofileCompleteDetails").find('input[name=EntrId]').val();
                profileCompleteDetailModel.CatId = $("#formprofileCompleteDetails").find('input[name=CatId]').val();
                profileCompleteDetailModel.Showfee = $("#formprofileCompleteDetails").find('input[name=Showfee]').val();
                profileCompleteDetailModel.PerfFee = $("#formprofileCompleteDetails").find('input[name=Performancefee]').val();
                profileCompleteDetailModel.PerfLength = $("#formprofileCompleteDetails").find('input[name=PerfLength]').val();
                profileCompleteDetailModel.YearsOfPerforming = $("#formprofileCompleteDetails").find('input[name=YearsOfPerforming]').val();
                profileCompleteDetailModel.PerfMembers = $("#formprofileCompleteDetails").find('input[name=PerfMembers]').val();
                profileCompleteDetailModel.NoOfShow = $("#formprofileCompleteDetails").find('input[name=NoofShow]').val();
                profileCompleteDetailModel.PerfLang = $("#formprofileCompleteDetails").find('input[name=PerfLanguage]').val();
                profileCompleteDetailModel.Gender = $("#formprofileCompleteDetails").find('input[name=Gender]').val();
                profileCompleteDetailModel.Nationality = $("#formprofileCompleteDetails").find('input[name=Nationality]').val();
                profileCompleteDetailModel.CityId = $("#formprofileCompleteDetails").find('input[name=CityID]').val();
                profileCompleteDetailModel.Mobile = $("#formprofileCompleteDetails").find('input[name=Mobile]').val();
                profileCompleteDetailModel.DonotShowfee = profileCompleteDetailModel.Showfee;

                $http.post('/Entertainer/MyAccount/UpdateProfileDetails', profileCompleteDetailModel).then(function (data) {
                    $("#formprofileCompleteDetails").find('img[class=imgLoader]').hide();
                    if (data.data == "Success") {
                        $("#formprofileCompleteDetails").find('a[id=openSuccessMsg]').trigger('click');
                    }
                }).catch(function () {
                    $("#formprofileCompleteDetails").find('img[class=imgLoader]').hide();
                });

            }
        }

        $scope.closeSuccessPopUp = function () {
            $window.location.href = "/Home/Index";
        }

        $scope.closeTSSuccessPopUp = function () {
            $window.location.href = "/Home/Index";
        }



    };

    //Add controller to the app
    app.controller("IndEntnJSController", IndEntnJSController);

}());

app.directive('nxEqual', function () {
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, model) {
            if (!attrs.nxEqual) {
                console.error('nxEqual expects a model as an argument!');
                return;
            }
            scope.$watch(attrs.nxEqual, function (value) {
                model.$setValidity('nxEqual', value === model.$viewValue);
            });
            model.$parsers.push(function (value) {
                var isValid = value === scope.$eval(attrs.nxEqual);
                model.$setValidity('nxEqual', isValid);
                return isValid ? value : undefined;
            });
        }
    };
});

app.directive('capitalize', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            var capitalize = function (inputValue) {
                if (inputValue == undefined) inputValue = '';
                var capitalized = inputValue.toLowerCase();
                capitalized = capitalized.replace(' ', '');
                if (capitalized !== inputValue) {
                    modelCtrl.$setViewValue(capitalized);
                    modelCtrl.$render();
                }
                return capitalized;
            }
            modelCtrl.$parsers.push(capitalize);
            capitalize(scope[attrs.ngModel]); // capitalize initial value
        }
    };
});

app.directive('ngUnique', function ($timeout, $q, $http) {

    return {
        restrict: 'AE',
        require: 'ngModel',
        link: function (scope, elm, attr, model) {
            //debugger;
            var val = elm.val();
            model.$asyncValidators.usernameExists = function () {
                return $http.post("/Account/CheckUserNameAlreadyExist", { username: model.$viewValue }).then(function (res) {
                    //debugger;
                    $timeout(function () {
                        if (res.data == "fail") {
                            model.$setValidity('unique', false);
                        } else {
                            model.$setValidity('unique', true);
                        }
                    }, 1000);
                });


            };
        }
    }
});

//app.directive('ngGetimgpreview',function ($compile) {
//    debugger;
//    return {        
//        link: function (scope, elem, attrs, ctrl) {

//            elem.bind('change', function (evt) {
//                debugger;

//                scope.files = [];
//                scope.$apply(function () {
//                    for (var i = 0; i < elem[0].files.length; i++) {
//                        scope.files.push(elem[0].files[i])
//                    }
//                    var filesAmount = elem[0].files.length;
//                    var counter = 0;
//                    for (i = 0; i < filesAmount; i++) {
//                        var reader = new FileReader();
//                        reader.onload = function (event) {
//                            debugger;
//                            var ImagePreviewHtml = "<div class='col-md-6 imageDiv' style='width: 20%;'>"
//                                                 + "<div class='trending-wrap' style='height: 150px;width: 150px;'>"
//                                                 + "<div class='trend_img' style='position:relative;'>"
//                                                 + "<span ng-click='deleteImageReg($event)' class='text-info text-right' style='position:absolute; padding:10px; right:0;'>"
//                                                 + "<a href='javascript:;' title='Delete Image' style='color:white;'>"
//                                                 + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
//                                                 + "</a>"
//                                                 + "</span>"
//                                                 + "<a class='video-pop-link imggal'>"
//                                                 + "<img src='" + event.target.result + "' id='img" + counter + "' style='height: 150px;width: 150px;padding: 4px;'/>"
//                                                 + "</a>"
//                                                 + "</div>"
//                                                 + "</div>"
//                                                 + "</div>";

//                            var content = $compile(ImagePreviewHtml)(scope);
//                            //element.append(content);

//                            $("#formprofileCompleteDetails").find('div[id=gallery]').append(content);
//                            //$compile(ImagePreviewHtml)(scope);

//                        }
//                        reader.readAsDataURL(elem[0].files[i]);
//                    }
//                });


//            });
//        }
//    }

//});



//app.directive('validFile', function () {
//    debugger;
//    return {
//        require: 'ngModel',
//        link: function (scope, elem, attrs, ngModel) {
//            var validFormats = ['jpg', 'jpeg', 'png'];
//            elem.bind('change', function () {
//                debugger;
//                validImage(false);
//                scope.$apply(function () {
//                    ngModel.$render();
//                });             
//            });
//            ngModel.$render = function () {
//                debugger;
//                ngModel.$setViewValue(elem.val());
//            };
//            function validImage(bool) {
//                debugger;
//                ngModel.$setValidity('extension', bool);
//            }
//            ngModel.$parsers.push(function (value) {
//                debugger;
//                var ext = value.substr(value.lastIndexOf('.') + 1);
//                if (ext == '') return;
//                if (validFormats.indexOf(ext) == -1) {
//                    return value;
//                }
//                validImage(true);
//                return value;
//            });
//        }
//    };
//});


//app.directive('ngUnique', ['$http', function ($timeout, $http) {
//    debugger;
//    return {
//        require: 'ngModel',
//        link: function (scope, elem, attrs, ctrl) {

//            elem.on('blur', function (evt) {
//                scope.$apply(function () {
//                    debugger;
//                    var val = elem.val();
//                    var req = { "username": val }

//                    //$http.post("/Account/CheckUserNameAlreadyExist", { username: val }).then(function (res) {
//                    //    debugger;
//                    //    $timeout(function () {
//                    //        if (res.data == "fail") {
//                    //            model.$setValidity('unique', false);
//                    //        } else {
//                    //            model.$setValidity('unique', true);
//                    //        }
//                    //        //model.$setValidity('usernameExists', !!res.data);
//                    //        //model.$setValidity('unique', false);
//                    //    }, 1500);
//                    //});

//                    //$.post("/Account/CheckUserNameAlreadyExist", { username: val }, function (result) {
//                    //    debugger;
//                    //    if (result) {
//                    //        setTimeout(function () {
//                    //            debugger;
//                    //        if (result == "fail") {
//                    //            ctrl.$setValidity('unique', false);
//                    //        } else {
//                    //            ctrl.$setValidity('unique', true);
//                    //        }
//                    //        }, 100);
//                    //    }
//                    //});

//                    //var ajaxConfiguration = { method: 'POST', url: '/Account/CheckUserNameAlreadyExist', data: req };
//                    //$asyncValidators(ajaxConfiguration).success(function (data, status, headers, config) {
//                    //    debugger;
//                    //        ctrl.$setValidity('unique', data.result);
//                    //    });
//                });
//            });
//        }
//    }
//}]);
