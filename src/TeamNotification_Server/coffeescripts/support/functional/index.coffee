partial = (func, args...) ->
    return (partial_args...) ->
        substituted_arguments = []
        for arg in args
            if not arg?
                argument_value = partial_args.shift()
            else
                argument_value = arg

            substituted_arguments.push argument_value

        func.apply(null, substituted_arguments)

curry = (func, args...) ->
    return (last_args...) ->
        func.apply(null, args.concat(last_args))

module.exports =
    partial: partial
    curry: curry
