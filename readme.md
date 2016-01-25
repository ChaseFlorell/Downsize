###Downsize###
Downsize is a simple helper tool to help you manage the images (jpg and png) in your cross-platform applications.

###Rational###
Whenever you create an application, you inevitably have have images as a part of your solution. The problem comes in the fact that there is so many screen resolutions for you to support, that managing the image assets can become very frustrating. That's where `Downsize` comes into play.

###What does it do?###
Simply put, Downsize takes your image and creates all of the required output image versions for both Android and iOS (Windows support coming).

###How do I use it?###
The easiest way is to simply drag and drop your image resource over top of the `downsize.exe`. It will automatically generate the required images for you.  
**BUT there's a catch**. Downsize will ALWAYS assume that you've given it the largest version of your file. That means that the original image is set to the iOS `@3x` or the Android `xxhdpi` versions. Downsize will then downsize your images to `@2x`, `@1x`, `xhdpi`, `hdpi`, `mdpi`, and `ldpi`. It will also appropriately name your original file and save it for `@3x` and `xxhdpi`.

###Example###
Given that you have the need for an image on your screen that is 40x40. The first thing you should do is calculate the xxhdpi/@3x resolution for that image. In this case it will be 120x120. So go ahead and create your image asset at that resolution. Once you've saved it out, simply drag-and-drop that image over top of the `Downsize.exe`, and watch as a new Android and iOS folder are created and all of the resolutions generated.

Need more power? You can pass additional variables at the command line as needed. Simply type `downsize.exe /?` to see what they are.

###More..###
Downsize can be used by itself, or with the additional config file. If you wish to customize the output format of your images, simply edit the config file and have it accompany your `Downsize.exe`

```xml
    <ImageCollection>
      <!--
	    key: Just for understanding which is which

		pathFormat:
          Index 0: root path
          Index 1: file name

		scale: The number to divide the original resolution by
      -->
      <add key="ldpi"     pathFormat="{0}\Android\drawable-ldpi\{1}"   scale="4" />
      <add key="mdpi"     pathFormat="{0}\Android\drawable-mdpi\{1}"   scale="3" />
      <add key="hdpi"     pathFormat="{0}\Android\drawable-hdpi\{1}"   scale="2" />
      <add key="xhdpi"    pathFormat="{0}\Android\drawable-xhdpi\{1}"  scale="1.5" />
      <add key="xxhdpi"   pathFormat="{0}\Android\drawable-xxhdpi\{1}" scale="1" />
      <add key="iOS@1x"   pathFormat="{0}\iOS\{1}"                     scale="3" />
      <add key="iOS@2x"   pathFormat="{0}\iOS\{1}@2x"                  scale="1.5" />
      <add key="iOS@3x"   pathFormat="{0}\iOS\{1}@3x"                  scale="1" />
    </ImageCollection>
```

###Contributing###
This is my first stab at this tool, and it might be missing feature X, or platform Y. If you want something that it can't currently provide, I love pull requests. If it's a bigger change, a fresh Issue goes along ways before PR.

###License###

[Microsoft Public License (Ms-PL)](http://www.microsoft.com/en-us/openness/licenses.aspx#MPL)