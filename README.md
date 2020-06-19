# Тестовое задание (разработчик .NET/SQL)

Разработать прототип клиент-серверной системы для регистрации входящих писем в компанию.

#### Предметная область:

В компании есть секретарь, в задачи которого входит регистрировать в систему все входящие электронные письма. При получении письма секретарь открывает программу, заполняет атрибуты письма, сохраняет письмо в системе. Дополнительно при запросе от сотрудника компании секретарь может открыть программу:
  - получить список всех писем
  - получить данные письма по идентификатору и изменить их при необходимости

#### Необходимо:

Разработать структуру БД, расширяемую с точки зрения предметной области. Разработать веб-сервис на C#, который будет работать с БД типа MS SQL Server без использования ORM. Ожидается реализация только через ADO.NET и хранимые процедуры. В архитектуру сервиса должны быть заложены модульность и расширяемость - возможность подключения реализации через различные ORM, другие типы БД без изменения моделей.

#### Дополнительно на стороне веб-сервиса поддержать сценарии:
  - поиск по дате регистрации за период
  - поиск по адресату
  - поиск по отправителю
  - поиск по тегу

#### Атрибуты письма:
  - название
  - дата регистрации
  - адресат
  - отправитель
  - теги
  - содержание
