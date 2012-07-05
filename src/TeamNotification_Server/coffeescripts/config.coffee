db_config =
    user: 'postgres'
    password: '1234'
    host: 'localhost'
    db_main: 'dtt_main'
    db_test: 'dtt_test'


env_db = if process.env.NODE_ENV is 'test' then db_config.db_test else db_config.db_main
db =
    connection_string: "postgres://#{db_config.user}:#{db_config.password}@#{db_config.host}/#{env_db}"

host = 'localhost'
port = 3000
site =
    host: host
    port: port
    client_ID: '1234'
    client_secret: 'secret'
    url: if port is 80 then host else "#{host}:#{port}"

module.exports =
    db: db
    site: site
