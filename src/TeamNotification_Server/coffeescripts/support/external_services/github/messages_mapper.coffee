mapper_paths = [
    './mappers/push'
    './mappers/comment'
    './mappers/fork'
    './mappers/pull_request'
    './mappers/issue'
]

mappers = (require path for path in mapper_paths)

map = (event_obj) ->
  (mapper.map event_obj for mapper in mappers when mapper.can_map event_obj)[0] ? null

module.exports =
  map: map
