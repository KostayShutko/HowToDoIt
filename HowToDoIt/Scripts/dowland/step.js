// Required for drag and drop file access
jQuery.event.props.push('dataTransfer');

// IIFE to prevent globals
(function () {

    var s;
    var obj;
    $('body').on('mouseenter', '.profile', function (evt) {
        obj = evt.target;
    });
    var Step = {

        settings: {
            bod: $("body"),
            img: $(".profile-avatar"),
            fileInput: $("#uploader")
        },

        init: function () {
            s = Step.settings;
            Step.bindUIActions();
        },

        bindUIActions: function () {

            var timer;

            s.bod.on("dragover", function (event) {
                clearTimeout(timer);
                if (event.currentTarget == s.bod[0]) {
                    Step.showDroppableArea();
                }

                // Required for drop to work
                return false;
            });

            s.bod.on('dragleave', function (event) {
                if (event.currentTarget == s.bod[0]) {
                    // Flicker protection
                    timer = setTimeout(function () {
                        Step.hideDroppableArea();
                    }, 200);
                }
            });

            s.bod.on('drop', function (event) {
                // Or else the browser will open the file
                event.preventDefault();
                Step.handleDrop(event.dataTransfer.files);
            });

            s.fileInput.on('change', function (event) {
                Step.handleDrop(event.target.files);
            });
        },

        showDroppableArea: function () {
            s.bod.addClass("droppable");
        },

        hideDroppableArea: function () {
            s.bod.removeClass("droppable");
        },

        saveFile: function (file) {
            var xhr = new XMLHttpRequest();
            xhr.open('POST', '/Instructions/Upload');
            xhr.setRequestHeader('X-FILE-NAME', 'file.name');
            var fd = new FormData
            fd.append("file", file)
            xhr.send(fd)

            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    var json = JSON.parse(xhr.responseText);
                    $('#infoimg').val(json["responseText"]);
                }
            };
        },

        handleDrop: function (files) {
            Step.hideDroppableArea();
            // Multiple files can be dropped. Lets only deal with the "first" one.
            var file = files[0];
            Step.saveFile(file);
            if (typeof file !== 'undefined' && file.type.match('image.*')) {
                Step.resizeImage(file, 256, function (data) {
                    Step.placeImage(data);
                });

            } else {

                alert("That file wasn't an image.");

            }

        },

        resizeImage: function (file, size, callback) {

            var fileTracker = new FileReader;
            fileTracker.onload = function () {
                Resample(
                    this.result,
                    size,
                    size,
                    callback
                );
            }
            fileTracker.readAsDataURL(file);

            fileTracker.onabort = function () {
                alert("The upload was aborted.");
            }
            fileTracker.onerror = function () {
                alert("An error occured while reading the file.");
            }

        },

        placeImage: function (data) {
            var entity = $(obj).find(".profile-avatar");
            entity.attr("src", data);
            /*$('.profile').mouseenter(function (evt) {
                var obj = evt.target;
                var entity = $(obj).find(".profile-avatar");
                entity.attr("src", data);
                $(".profile").unbind("mousemove");
            });*/
        }

    }

    Step.init();

})();