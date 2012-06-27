db_config =
    user: 'postgres'
    password: '1234'
    host: 'localhost'
    db_main: 'dtt_main'
    db_test: 'dtt_test'


env_db = if process.env.NODE_ENV is 'test' then db_config.db_test else db_config.db_main
db =
    connection_string: "postgres://#{db_config.user}:#{db_config.password}@#{db_config.host}/#{env_db}"

module.exports =
    db: db
