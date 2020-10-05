# Задание

Разработать сервис отвечающий за ведение данных студентов.

## Сервис ответственен за сущности

1. Студент
    * ID (required, PK). Можно использовать любой вариант. Это может быть как GUID, так и обычный int/long + increment. Это поле системное, изменение данного поля не допускается.
    * Пол (обязательное поле).
    * Фамилия (обязательное, максимальная длина 40 символов)
    * Имя (обязательное, максимальная длина 40 символов)
    * Отчество ( не обязательное, максимальная длина 60 символов)
    * Уникальный идентификатор студента (not required, должен быть уникальным в рамках всех студентов, минимальная длина 6 символов, максимальная длина 16). Опциональный, например мы хотим задать для студента позывной, но не хотим чтобы два студента были с одинаковыми позывными.
2. Группа
    * ID (required, PK). Можно использовать любой вариант. Это может быть как GUID, так и обычный int/long + increment. Это поле системное, изменение данного поля не допускается.
    * Имя группы. (обязательное, максимальная длина 25 символов)

Студент может находится одновременно в нескольких группах (многие ко многим).

## Предусмотреть все виды операций над сущностями (CRUD)

Контроллеры должны содержать следующие методы:

1. Создание/удаление/редактирование студента
2. Создание/удаление/редактирование группы.
3. Добавление/Удаление студента из группы.
4. Получение списка студентов с возможностью фильтрации по полям (Пол, ФИО, уникальный идентификатор, название группы) и ограничением по результату (пагинация).
    * Результатом должен список где каждый элемент содержит поля: ID, ФИО, уникальный идентификатор, список групп через запятую.
5. Получение списка групп с возможностью фильтрации по полю «Имя группы».
    * Результатом должен список где каждый элемент содержит поля: ID, Имя группы, кол-во студентов в группе.

## Общие требования к коду

1. Dotnet core 2.0 +
2. EF
3. JSON как формат отдачи данных.

## Дополнительно

1. Docker
2. Swagger
3. Unit testing.
4. Защитить контроллеры с помощью JWT.

Выделить как минимум три слоя:

1. Domain Layer
2. Data Layer
3. Presentation Layer

Попытаться соблюсти правила зависимостей Clean Architecture https://proandroiddev.com/clean-architecture-data-flow-dependency-rule-615ffdd79e29

1. Presentation Layer depends on Domain Layer.
2. Data Layer depends on Domain Layer.
3. Domain Layer does NOT depend on Data Layer.

## Комментарии

На данный момент:

* Тесты сделаны по остаточному принципу
* Отсутствует актуальный фронтенд
* Отсутствует логирование
* Отсутствуют проверки данных в бд, подробные сообщения об ошибках/коллизиях и т.п.
* Отсутствует авторизация

Следующее на очереди: добавление фронта, авторизации и логирования