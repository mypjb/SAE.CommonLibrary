# SAE.CommonLibrary
通用类库

## RoadMap

 - 分表分库

   分表分库应该在高层进行区分，而非底层，高层据有更高层次的业务逻辑，请求越到底层，和业务越脱钩（底层通常具有通用性）。

 - ABAC

   基于属性的访问控制（比如说用户信息、设备信息、请求参数等）。每个端点具有认证策略，策略可以在后台进行动态维护。通过装饰器模式添加新的策略，通过策略模式动态添加操作符（>、<、<=、>=、==、!=），通过组合模式将规则两两结合（and、or）。
   每个系统只加载自身系统的授权配置，配置永远在内存中，避免网络传输带来的延迟和独立性。规则已经定义好，便于第三方系统接入。
  
   装饰器充当执行器，职责链负责对装饰器的细节进行构建

   后期可以尝试拦截请求参数对授权`属性`进行扩展。在进行分段解析的时候，可以先将属性排在前面统一进行解析，然后再是操作符
   abcdef > < &&

   先使用属性表达式，再使用关系表达式获取。

   ```json
   //user info
   user:
     {
      name: "admin",
      age: 18,
      role: student
     }
   //endpoint
   //default strategy: age > 18 || role == student
   {
    strategys:[
      {
        name: "age",
        operator: ">"
        value: "18"
      },
      {
        name: "role",
        operator: "==",
        value: "student"
      }
    ]
   }

    //mixed strategy: age > 18 && (role == student || name == admin)
   {
    strategys:[
      {
        name: "age",
        operator: ">"
        value: "18"
        type: "and"
      },
      {
        name: "role",
        operator: "==",
        value: "student"
      },
      {
        name: "name",
        operator: "==",
        value: "admin"
      }
    ]
   }
   ```