db = 
    user: 'postgres'
    password: '1234'
    host: 'localhost'
    main: 'dtt_main'
    test: 'dtt_test'
    connection_string: "postgres://#{@user}:#{@password}@#{@host}"
    get_connection_string_for: (db) ->
        "#{@connection_string}/#{db}"

module.exports =
    db: db
