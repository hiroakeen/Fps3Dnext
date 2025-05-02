using UnityEngine;

public class HitboxPart : MonoBehaviour
{
    public enum BodyPart { Head, Body }
    public BodyPart part = BodyPart.Body;

    public int GetDamage()
    {
        return part == BodyPart.Head ? 3 : 1; // �w�b�h�V���b�g��3�_���[�W
    }
}
