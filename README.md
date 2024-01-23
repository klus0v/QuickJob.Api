### Как примерно все устроенно:
![изображение](https://github.com/klus0v/QuickJob.Settings/assets/72307503/7ea2f1be-05b0-4c6f-a92e-74e4679f980f)

### Структура 
1. [QuickJob.Settings](https://github.com/klus0v/QuickJob.Settings) - приватный репо со всеми настройками (каждое апи в своей ветке)
2. [QuickJob.Cabinet.Api](https://github.com/klus0v/QuickJob.Cabinet.Api) - АПИ для работы с профилем пользоваетеля
   * Менять/ставить инфо о пользователе, аватарка, почты, ТГ - управление каналом уведомлений
   * [Дока](http://51.250.93.99:4444/swagger/index.html), [АвтосгенеренныйTS](https://github.com/klus0v/QuickJob.Settings/blob/ts/Script/ScriptResults/cabinet-apiClient.ts)
   * Можно кидать запросы сюда `http://51.250.93.99/cabinet-api`
3. [QuickJob.Users.Api](https://github.com/klus0v/QuickJob.Users.Api) - АПИ для регистрации и входа
   * Рега с подтверждением почты (хак: можно при подтверждениии отправить код `0000` и он всегда подойдет - для удобства тестирования)
   * Вход логин+пароль, JWT токены, ревреш токен ставится как сессионая кука, с ним ничего делать не надо
   * [Дока](http://51.250.93.99:5555/swagger/index.html), [АвтосгенеренныйTS](https://github.com/klus0v/QuickJob.Settings/blob/ts/Script/ScriptResults/users-apiClient.ts)
   * Можно кидать запросы сюда `http://51.250.93.99/users-api`
   * Есть методы с `users/...` ' это для других апи (доступ по апи ключу, не токену) - по идее нужно в отдельное апи, но уже лень
4. [QuickJob.Api](https://github.com/klus0v/QuickJob.Api) - основне АПИ со всей бизнесс логикой
   * Создание и работы с вакансиями, работа с откликами
   * Поиск и отклик на вакансии
   * Принять/отклонить отклики
   * [Дока](http://51.250.93.99:7777/swagger/index.html), [АвтосгенеренныйTS](https://github.com/klus0v/QuickJob.Settings/blob/ts/Script/ScriptResults/apiClient.ts)
   * Можно кидать запросы сюда `http://51.250.93.99/api`

[админка keycloak](http://51.250.93.99:8080/) - если нужно можно посмотреть пользователей, поредачить и прочее (пароль и логин [тут](https://github.com/klus0v/QuickJob.Settings/blob/users-api-settings/KeycloackSettings.json#L12-L13))

 [QuickJob.Notifications.Api](https://github.com/klus0v/QuickJob.Notifications.Api) - внутренне апи для отправки уведомлений (доступ по апи ключу, не токену) 

 ### TypeScript
###### Скрипт для генарации из json сваггера ts файлов живет в [ветке ts](https://github.com/klus0v/QuickJob.Settings/tree/ts/Script)

###### чтобы обновить ts файлы самому:
1. Зайти в папку Script
2. Запустить `dotnet run`
3. Нужен dotnet8 (если стоит шестой, можно изменить конфиг скрипта [тут](https://github.com/klus0v/QuickJob.Settings/blob/ts/Script/Script.csproj#L5) (net8 => net6) )
4. Результаты будут [тут](https://github.com/klus0v/QuickJob.Settings/tree/ts/Script/ScriptResults)

###### последние сгенеранные файлы: https://github.com/klus0v/QuickJob.Settings/tree/ts/Script/ScriptResults
