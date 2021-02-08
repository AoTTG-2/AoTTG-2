using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public partial class Hero : IDebugable
{
    public string GetDebugString(StringBuilder sb)
    {
        sb.Append("Position:")
            .Append("\t\tx: ").Append(transform.position.x).AppendLine()
            .Append("\t\t\ty: ").Append(transform.position.y).AppendLine()
            .Append("\t\t\tz: ").Append(transform.position.z).AppendLine();
        sb.AppendLine();
        sb.Append("Rotation:")
            .Append("\t\tx: ").Append(transform.eulerAngles.x).AppendLine()
            .Append("\t\t\ty: ").Append(transform.eulerAngles.y).AppendLine()
            .Append("\t\t\tz: ").Append(transform.eulerAngles.z).AppendLine();
        sb.AppendLine();
        sb.Append("State:\t\t\t").Append(State).AppendLine();
        sb.Append("Team:\t\t\t").Append(myTeam).AppendLine();


        sb.AppendLine();
        sb.Append("Left:\t\t\t").Append(isLeftHandHooked).Append(" ");

        if (isLeftHandHooked && (bulletLeft != null))
        {
            var vector = bulletLeft.transform.position - transform.position;
            var num = ((int) (Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg));
            sb.Append(num);
        }

        sb.AppendLine();
        sb.Append("Right:\t\t\t").Append(isRightHandHooked).Append(" ");

        if (isRightHandHooked && (bulletRight != null))
        {
            var vector = bulletRight.transform.position - transform.position;
            var num = ((int) (Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg));
            sb.Append(num);
        }
        sb.AppendLine();
        sb.AppendLine();

        sb.Append("Facing Direction:\t").Append((int) facingDirection).AppendLine();
        sb.Append("Real Facing Direction:\t").Append((int) transform.rotation.eulerAngles.y).AppendLine();
        sb.AppendLine().AppendLine().AppendLine().AppendLine();

        if (State == HERO_STATE.Attack)
        {
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        }
        return sb.ToString();
    }
}
