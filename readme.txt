1) БД называется btcgatewayapi, название не привязано жестно, можно любое.
2) в файле db.sql - схема БД и данные для для таблицы clients (логин rex, пароль 1qaz!QAZ)
3) Аутентификация. Для получения Bearer-токена нужно выполнить POST-запрос на http://localhost:60542/token с типом 
	данных Form Url Encoded.
	Поля запроса: 
	grant_type: password
	username: rex
	password: 1qaz!QAZ

