# ToolboxExample

Plugin d'exemple pour tester rapidement les API de Toolbox.

Commandes utiles:

- `/tbxhelp`: liste les commandes.
- `/tbxprofile`: affiche les informations joueur.
- `/tbxitems`: teste items, NBT, inventaire, armure et cooldown.
- `/tbxforms`: ouvre une simple form, une modal et une custom form.
- `/tbxinv`: ouvre un faux inventaire interactif avec callbacks, valeurs et mise a jour.
- `/tbxui`: teste title, toast, scoreboard, HUD et input locks.
- `/tbxworld`: teste monde, blocks, entites et dimensions.
- `/tbxtask`: lance des tasks immediate, delayed et repeating.
- `/tbxpacketlog`: active/desactive l'inspection des packets dans la console.
- `/tbxkit <count> [name]`: exemple de commande avec parametres.
- `/tbxeffect <speed|jumpboost|nightvision> [seconds]`: autre commande avec parametres.
- `/tbxecho <message...>`: exemple de `Cmd.Varargs`.

Logs:

- Toolbox ecrit les erreurs dans la console du serveur.
- Toolbox ecrit aussi les erreurs dans `logs/toolbox-errors.log`, depuis le dossier ou le serveur est lance.
- Les forms, commandes, events et tasks sont proteges pour eviter qu'une exception C# disparaisse sans trace.
