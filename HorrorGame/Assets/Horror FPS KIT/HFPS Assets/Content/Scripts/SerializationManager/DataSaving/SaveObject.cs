using System.Collections.Generic;
using UnityEngine;
using ThunderWire.Parser;

public class SaveObject : MonoBehaviour {

    ParsePrimitives parser = new ParsePrimitives();

    public enum SaveType { PositionRotation, Position, Rotation, RendererActive, ObjectActive, Door, Drawer, Lever, Valve, MovableInteract, Keypad, Light, NPC }
    public SaveType saveType = SaveType.PositionRotation;
    public GameObject CustomObjectActive;

    [Tooltip("Unique name under which data will be saved. Name cannot be duplicate of other!!")]
    public string uniqueName;

    public Dictionary<string, List<string>> GetObjectData()
    {
        Dictionary<string, List<string>> objectData = new Dictionary<string, List<string>>();

        if (saveType == SaveType.PositionRotation)
        {
            objectData.Add(uniqueName, new List<string>{
            gameObject.GetComponent<MeshRenderer>().enabled.ToString(),
            transform.position.x.ToString(),
            transform.position.y.ToString(),
            transform.position.z.ToString(),
            transform.eulerAngles.x.ToString(),
            transform.eulerAngles.y.ToString(),
            transform.eulerAngles.z.ToString()
        });
        }
        else if(saveType == SaveType.Position)
        {
            objectData.Add(uniqueName, new List<string>{
            GetObjectEnabled(gameObject),
            transform.position.x.ToString(),
            transform.position.y.ToString(),
            transform.position.z.ToString()
            });
        }
        else if(saveType == SaveType.Rotation)
        {
            objectData.Add(uniqueName, new List<string>{
            GetObjectEnabled(gameObject),
            transform.eulerAngles.x.ToString(),
            transform.eulerAngles.y.ToString(),
            transform.eulerAngles.z.ToString()
            });
        }
        else if(saveType == SaveType.RendererActive)
        {
            objectData.Add(uniqueName, new List<string>{
            GetObjectEnabled(gameObject),
            });
        }
        else if (saveType == SaveType.ObjectActive)
        {
            objectData.Add(uniqueName, new List<string>{
            CustomObjectActive.activeSelf.ToString()
            });
        }
        else if(saveType == SaveType.Door)
        {
            objectData.Add(uniqueName, new List<string>{
            GetComponent<DynamicObject>().useType.ToString(),
            GetComponent<DynamicObject>().angle.ToString(),
            GetComponent<DynamicObject>().isOpen.ToString(),
            GetComponent<DynamicObject>().isUnlocked.ToString()
            });
        }
        else if (saveType == SaveType.Drawer)
        {
            objectData.Add(uniqueName, new List<string>{
            GetComponent<DynamicObject>().useType.ToString(),
            transform.position.x.ToString(),
            transform.position.z.ToString(),
            GetComponent<DynamicObject>().isOpen.ToString(),
            GetComponent<DynamicObject>().isUnlocked.ToString()
            });
        }
        else if (saveType == SaveType.Lever)
        {
            objectData.Add(uniqueName, new List<string>{
            GetComponent<DynamicObject>().useType.ToString(),
            GetComponent<DynamicObject>().leverAngle.ToString(),
            GetComponent<DynamicObject>().isUp.ToString(),
            GetComponent<DynamicObject>().isHolding.ToString(),
            GetComponent<DynamicObject>().hold.ToString()
            });
        }
        else if(saveType == SaveType.Valve)
        {
            objectData.Add(uniqueName, new List<string>{
            GetComponent<DynamicObject>().rotateValue.ToString(),
            GetComponent<DynamicObject>().isRotated.ToString()
            });
        }
        else if (saveType == SaveType.MovableInteract)
        {
            objectData.Add(uniqueName, new List<string>{
            GetComponent<DynamicObject>().isInvoked.ToString()
            });
        }
        else if (saveType == SaveType.Keypad)
        {
            objectData.Add(uniqueName, new List<string>{
            GetComponent<Keypad>().m_accessGranted.ToString(),
            });
        }
        else if (saveType == SaveType.Light)
        {
            objectData.Add(uniqueName, new List<string>{
            GetComponent<Lamp>().switchLight.enabled.ToString()
            });
        }
        else if (saveType == SaveType.NPC)
        {
            List<string> data = GetComponent<ZombieAI>().GetZombieData();
            objectData.Add(uniqueName, data);
        }

        return objectData;
    }

    public void SetObjectData (string[] objectData)
    {
        if (objectData.Length > 0)
        {
            if (saveType == SaveType.PositionRotation)
            {
                DisableObject(gameObject, parser.ParseType<bool>(objectData[0]));

                float y = parser.ParseType<float>(objectData[2]);
                Vector3 pos = parser.ParseVector3(objectData[1], y.ToString(), objectData[3]);
                Vector3 rot = parser.ParseVector3(objectData[4], objectData[5], objectData[6]);

                transform.position = pos;
                transform.eulerAngles = rot;
            }
            else if (saveType == SaveType.Position)
            {
                DisableObject(gameObject, parser.ParseType<bool>(objectData[0]));

                float y = parser.ParseType<float>(objectData[2]);
                Vector3 pos = parser.ParseVector3(objectData[1], y.ToString(), objectData[3]);

                transform.position = pos;
            }
            else if (saveType == SaveType.Rotation)
            {
                DisableObject(gameObject, parser.ParseType<bool>(objectData[0]));

                Vector3 rot = parser.ParseVector3(objectData[4], objectData[5], objectData[6]);

                transform.eulerAngles = rot;
            }
            else if (saveType == SaveType.RendererActive)
            {
                DisableObject(gameObject, parser.ParseType<bool>(objectData[0]));
            }
            else if (saveType == SaveType.ObjectActive)
            {
                CustomObjectActive.SetActive(parser.ParseType<bool>(objectData[0]));
            }
            else if (saveType == SaveType.Door)
            {
                Vector3 rot = new Vector3(transform.eulerAngles.x, parser.ParseType<float>(objectData[1]), transform.eulerAngles.z);
                GetComponent<DynamicObject>().ParseUseType(objectData[0]);
                transform.eulerAngles = rot;
                GetComponent<DynamicObject>().angle = parser.ParseType<float>(objectData[1]);
                GetComponent<DynamicObject>().isOpen = parser.ParseType<bool>(objectData[2]);
                GetComponent<DynamicObject>().isUnlocked = parser.ParseType<bool>(objectData[3]);
            }
            else if (saveType == SaveType.Drawer)
            {
                GetComponent<DynamicObject>().ParseUseType(objectData[0]);
                Vector3 pos = new Vector3(parser.ParseType<float>(objectData[1]), transform.position.y, parser.ParseType<float>(objectData[2]));
                transform.position = pos;
                GetComponent<DynamicObject>().isOpen = parser.ParseType<bool>(objectData[3]);
                GetComponent<DynamicObject>().isUnlocked = parser.ParseType<bool>(objectData[4]);
            }
            else if (saveType == SaveType.Lever)
            {
                bool reverse = GetComponent<DynamicObject>().reverseMove;
                GetComponent<DynamicObject>().ParseUseType(objectData[0]);
                if (reverse)
                {
                    Vector3 rot = new Vector3(transform.localEulerAngles.y, parser.ParseType<float>(objectData[1]), transform.localEulerAngles.z);
                    transform.localEulerAngles = rot;
                }
                else
                {
                    Vector3 rot1 = new Vector3(transform.localEulerAngles.x, parser.ParseType<float>(objectData[1]), transform.localEulerAngles.z);
                    transform.localEulerAngles = rot1;
                }
                GetComponent<DynamicObject>().isUp = parser.ParseType<bool>(objectData[2]);
                GetComponent<DynamicObject>().isHolding = parser.ParseType<bool>(objectData[3]);
                GetComponent<DynamicObject>().hold = parser.ParseType<bool>(objectData[4]);
            }
            else if (saveType == SaveType.Valve)
            {
                GetComponent<DynamicObject>().rotateValue = parser.ParseType<float>(objectData[0]);
                GetComponent<DynamicObject>().isRotated = parser.ParseType<bool>(objectData[1]);
            }
            else if (saveType == SaveType.MovableInteract)
            {
                GetComponent<DynamicObject>().isInvoked = parser.ParseType<bool>(objectData[0]);
            }
            else if (saveType == SaveType.Keypad)
            {
                bool granted = parser.ParseType<bool>(objectData[0]);
                if (granted)
                {
                    GetComponent<Keypad>().SetAccessGranted();
                }
            }
            else if (saveType == SaveType.Light)
            {
                bool isOn = parser.ParseType<bool>(objectData[0]);
                GetComponent<Lamp>().isOn = isOn;
                GetComponent<Lamp>().switchLight.enabled = isOn;
                GetComponent<Lamp>().OnLoad();
            }
            else if (saveType == SaveType.NPC)
            {
                bool isDead = parser.ParseType<bool>(objectData[0]);
                bool patrol = parser.ParseType<bool>(objectData[7]);
                bool patrol_point = parser.ParseType<bool>(objectData[8]);
                if (isDead)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    transform.localPosition = parser.ParseVector3(objectData[1], objectData[2], objectData[3]);
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, parser.ParseType<float>(objectData[15]), transform.eulerAngles.z);
                    GetComponent<NPCHealth>().Health = parser.ParseType<int>(objectData[4]);
                    GetComponent<ZombieAI>().isAttracted = parser.ParseType<bool>(objectData[5]);
                    GetComponent<ZombieAI>().path = parser.ParseType<int>(objectData[6]);
                    if (patrol_point)
                    {
                        GetComponent<ZombieAI>().GoZombiePatrolPoint(
                            parser.ParseVector3(objectData[9], objectData[10], objectData[11]),
                            parser.ParseVector3(objectData[12], objectData[13], objectData[14])
                            );
                    }
                    else
                    {
                        if (patrol)
                        {
                            GetComponent<ZombieAI>().GoZombiePatrol();
                        }
                    }
                }
            }
        }
    }

    string GetObjectEnabled(GameObject obj)
    {
        return obj.GetComponent<MeshRenderer>().enabled.ToString();
    }

    void DisableObject(GameObject obj, bool active)
    {
        if (active == false)
        {
            if (obj.GetComponent<ItemID>())
            {
                obj.GetComponent<ItemID>().DisableObject(active);
            }
            else
            {
                obj.GetComponent<MeshRenderer>().enabled = false;
                obj.GetComponent<Collider>().enabled = false;
            }
        }
    }
}