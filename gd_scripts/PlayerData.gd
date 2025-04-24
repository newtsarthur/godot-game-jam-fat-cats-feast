extends Node
class_name PlayerData

signal items_collected_in_level_changed(new_value: int)

var items_collected_total := 0
var _items_collected_in_level := 0
var death_count := 0
var inicio := 0

static var instance: PlayerData

# A variável pública com setget
var items_collected_in_level := 0:
	get:
		return _items_collected_in_level
	set(value):
		_items_collected_in_level = value
		emit_signal("items_collected_in_level_changed", _items_collected_in_level)

func _ready() -> void:
	instance = self

func reset_level_progress() -> void:
	items_collected_in_level = 0
	inicio = 1

func commit_level_progress() -> void:
	items_collected_total += items_collected_in_level
	items_collected_in_level = 0
