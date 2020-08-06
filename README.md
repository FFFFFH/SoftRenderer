# SoftwareRenderer
基于C#实现的软光栅器，采用Unity的COM模式，旨在模拟Unity渲染管线。
目前已实现：
1、线框模式、纹理模式、顶点色模式

2、背面消隐

3、Cvv简单裁剪

4、纹理uv坐标和顶点色等的透视校正插值

5、纹理双线性过滤采样

TODO：

1.实现简单光照。

2.支持贴图缩放偏移。

3.屏幕空间分割裁剪。

4.重心法实现三角形插值。

5.引入颜色缓冲，实现Blending。

效果图：

![image](https://github.com/lnusie/SoftwareRenderer/blob/master/SoftRenderer/pictures/Wireframe.png)

(线框模式)

![image](https://github.com/lnusie/SoftwareRenderer/blob/master/SoftRenderer/pictures/Fill.png.png)

(填充模式)

![image](https://github.com/lnusie/SoftwareRenderer/blob/master/SoftRenderer/pictures/Color.png)

(顶点色模式)

![image](https://github.com/lnusie/SoftwareRenderer/blob/master/SoftRenderer/pictures/Texture.png)

(贴图模式)




