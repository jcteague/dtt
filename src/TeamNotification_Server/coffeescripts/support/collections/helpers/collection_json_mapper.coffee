mappers = require('./mappers')

map = (raw_collection) ->
    standard_collection =
        href: raw_collection.href
        version: "0.1"

    for key, val of raw_collection
        if mappers["#{key}_collection_field_mapper"]?
            search_key = "#{key}_collection_field_mapper"
        else
            search_key = "default_collection_field_mapper"

        standard_collection[key] = mappers[search_key].map val, raw_collection

    return {
        collection: standard_collection
    }

    ###
    return {
        collection:
            version: "0.1"

            links: map_links_from(raw_collection.links)
            items: map_items_from(raw_collection)
            queries: map_queries_from(raw_collection.queries)
            template: map_template_from(raw_collection.template)
            error: map_error_from(raw_collection.error)
    }
    ###

module.exports =
    map: map
