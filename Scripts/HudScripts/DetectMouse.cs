using Godot;
using System;

public class DetectMouse : Container
{
	public override void _Notification(int notif)
	{
		switch (notif)
		{
			case NotificationMouseEnter:
				HUDManager.Player.IsMouseOverHUD = true;
				break;
			case NotificationMouseExit:
				HUDManager.Player.IsMouseOverHUD = false;
				break;
		}
	}
}
