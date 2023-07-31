# SAE.CommonLibrary
通用类库

## RoadMap

 - 分表分库

   分表分库应该在高层进行区分，而非底层，高层据有更高层次的业务逻辑，请求越到底层，和业务越脱钩（底层通常具有通用性）。

 - ABAC

   基于属性的访问控制。每个端点具有认证策略，策略可以在后台进行动态维护。通过装饰器模式添加新的策略，通过策略模式动态添加操作符（>、<、<=、>=、==、!=），通过组合模式将规则两两结合（and、or）。
  
  
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