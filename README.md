# MyLab.RemoteConfig

[![NuGet](https://img.shields.io/nuget/v/MyLab.RemoteConfig.svg)](https://www.nuget.org/packages/MyLab.RemoteConfig/)

**.NET Core 2.2** библиотека для загрузки удалённой конфигурации по `HTTP` протоколу с `Basic` авторизацией.

## Применение

### Подключение

Начните с подключения библиотеки через nuget пакет:

```
Install-Package MyLab.RemoteConfig -Version x.x.x
```

Для активации загрузки удалённой конфигурации необходимо воспользоваться методом расширения `AddRemoteConfiguration` для `IWebHostBuidler`:

```C#
public class Program
{
    public static void Main(string[] args)
    {
       CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
           .AddRemoteConfiguration() //<----- here
           .UseStartup<Startup>();
}
```

### Опциональность

Есть поддержка случая, когда удалённый конфиг - опциональная возможность. В этом случае, нужно указать параметр `optional: true`:

```C#
WebHost.CreateDefaultBuilder(args)
    .AddRemoteConfiguration(optional:true) //<----- here
    .UseStartup<Startup>();
```

По умолчанию, при использовании удалённой конфигурации, она обязательная.

### Конфигурирование

#### app.settings

Также, необходимо указать параметры подключения в конфигурационном файле:

```json
{
  "RemoteConfig": {
    "Url": "http://localhost",
    "Host": "localhost",
    "User": "foo",
    "Password": "foo-pass"
  }
}
```

**Важно!**

Должен быть указан хотя бы один из параметров: `Url` или `Host`. Если указан `Url`, то используется как есть в качестве адреса. В противном случае испольщуется параметр `Host` для построения адреса по шаблону 

```
http://{host}/api/condig
```

Например для конфигурации

```json
{
  "RemoteConfig": {
    "Host": "hot.com:3434",
    "User": "foo",
    "Password": "foo-pass"
  }
}
```

результирующий адрес будет иметь вид

```
http://hot.com:3434/api/condig
```


#### Переменные окружения

Альтернативный способ - указание параметров подключения через переменные окружения.

Для этого необходимо указать переменные окружения:

* MYLAB_REMOTECONFIG\__URL или  MYLAB_REMOTECONFIG__HOST
* MYLAB_REMOTECONFIG__USER
* MYLAB_REMOTECONFIG__PASSWORD

Для использования этих переменных окружения следует использовать метод расширения `LoadRemoteConfigConnectionFromEnvironmentVars` для `IWebHostBuidler`:

```C#
public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
```
```C#

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .LoadRemoteConfigConnectionFromEnvironmentVars() // <-- here
                .AddRemoteConfiguration()
                .UseStartup<Startup>();
    }
```

## PS

Сервис предоставления удалённой конфигурации - [MyLab.ConfigServer](https://github.com/ozzy-ext-mylab/config-server).