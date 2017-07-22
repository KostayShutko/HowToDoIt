var Resample = (function (canvas) {

    // (C) WebReflection Mit Style License

    function Resample(img, width, height, onresample) {
        var
            load = typeof img == "string",
            i = load || img;

        // if string, a new Image is needed
        if (load) {
            i = new Image;
            i.onload = onload;
            i.onerror = onerror;
        }

        i._onresample = onresample;
        i._width = width;
        i._height = height;
        load ? (i.src = img) : onload.call(img);
    }

    function onerror() {
        throw ("not found: " + this.src);
    }

    function onload() {
        var
            img = this,
            width = img._width,
            height = img._height,
            onresample = img._onresample
            ;
        maxWidth = parseInt($(".profile").css("width"), 10);
        maxHeight = parseInt($(".profile").css("height"), 10);

        var ratioX = maxWidth / img.width;
        var ratioY = maxHeight / img.height;
        var ratio = Math.min(ratioX, ratioY);
        var newWidth = (img.width * ratio);
        var newHeight = (img.height * ratio);
        // if width and height are both specified
        // the resample uses these pixels
        // if width is specified but not the height
        // the resample respects proportions
        // accordingly with orginal size
        // same is if there is a height, but no width
        var minValue = Math.min(img.height, img.width);
        width == null && (width = round(img.width * height / img.height));
        height == null && (height = round(img.height * width / img.width));

        delete img._onresample;
        delete img._width;
        delete img._height;

        // when we reassign a canvas size
        // this clears automatically
        // the size should be exactly the same
        // of the final image
        // so that toDataURL ctx method
        // will return the whole canvas as png
        // without empty spaces or lines
        canvas.width = newWidth;
        canvas.height = newHeight;
        
        // drawImage has different overloads
        // in this case we need the following one ...
        context.drawImage(img, 0, 0, newWidth, newHeight);
        
        // retrieve the canvas content as
        // base4 encoded PNG image
        // and pass the result to the callback
        onresample(canvas.toDataURL("image/png"));
    }

    var context = canvas.getContext("2d"),
        // local scope shortcut
        round = Math.round
        ;

    return Resample;

}(
    this.document.createElement("canvas"))
);