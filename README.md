# assets-query使用说明
pull项目后，如果只想使用，可以在将./Build/目录下的.unitypackage引入项目即可
## 冗余图片资源
### 工程配置
冗余图片查询功能入口Window/AssetsQuery。在使用之前需要先配置工程文件，Assets/Editor/AssetsQuery/AssetsQueryRules.asset文件。
- 配置Prefab Root Directory Data为prefab根目录。
- 配置Image Root Directory Data为图片根目录
- Img White List为白名单类型，在查询未使用图片过程中会直接跳过白名单内容。白名单包含两类型
    - 1.相对图片根目录的目录类型
    - 2.相对图片根目录下的文件路径
### 查询使用
打开AssetsQuery资源查找器窗口，点击查询未使用的图片。会检索到未使用的图片，弹出的界面可以放弃本次删除，也可以直接放进白名单。点击删除即可删除这些资源。
### 监视器IMG_Monitor
在图片查找中包含一个图片监视器，这是为了方便更加精准的查找资源而做的，因为很多时候会在代码中动态设置图片资源到场景中，因此静态查找不一定准确。
如果需要使用，在启动场景放一个空GameObject然后将ImageAssetsRuntimeMonitor.cs这个脚本挂上去。程序运行时会定期检查资源，并记录到EditorPref中，因此注意，如果换台机器是没有记录的。

# 类型说明
### AssetQueryRules.asset规则中的宏说明
#### Relative Type

在选择目录的时候会遇到选相对类型
- Assets：相对工程文件Assets目录下
- Project：相对工程目录，一般都是Assets目录的同级目录 

#### Editor Language
这个工具引入了一个简单的多语言工具，如果需要可以进行配置，目前翻译只有中文的。如果需要自行添加。
# 已完成功能
- 冗余图片查询工具
# 更新计划


