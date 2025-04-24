extends Node2D

@export var meu_label: Label  # Arraste o Label no editor

func _ready() -> void:
	# Conecta ao sinal emitido por PlayerData (autoload)
	PlayerDataTw.connect("items_collected_in_level_changed", Callable(self, "_on_items_collected_changed"))

	update_clones_display()

func _on_items_collected_changed(new_value: int) -> void:
	update_clones_display()

func update_clones_display() -> void:
	if meu_label:
		var points := PlayerDataTw.items_collected_total + PlayerDataTw.items_collected_in_level
		meu_label.text = str(points)
	else:
		push_error("Label não atribuído no HUD!")
