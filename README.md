# CXUtils For Unity By CXRedix

#### This is a **coding helper** for **Unity** that I made for ***simplifying*** the process of making applications / software in Unity.

---

## ***Wiki***:
 + [***Attributes***](#attributes)
 + [***Generics***](#generics)
 + [***Helper Components***](#helper-components)
 + [***Helper Utils***](#helper-utils)
 + [***Managers***](#managers)
 + [***Path Finding***](#path-finding)
 + [***Plane System***](#plane-system)
 + [***Pooler***](#pooler)
 + [***Timer***](#timer)
 + [***Useful Types***](#useful-types)

---

#### Attributes:
 + The Attributes in CXUtils are useful for testing in unity editor
 + [**BackgroundColorAttribute**](#BackgroundColorAttribute)
 + [**ForeColorAttribute**](#ForeColorAttribute)
 + [**LabelAttribute**](#LabelAttribute)
 + [**NotNullAttribute**](#NotNullAttribute)
 
 + ### BackgroundColorAttribute
 + set's the background color of a field inside unity's inspector window.
 
 ```csharp
 [BackgroundColor(string hexColor, bool onlyThisField = false)]
 ```

 ```csharp
 // sets the background color into white (#FFFFFF) and also tells it to set to this field only
 [BackgroundColor("FFFFFF", true)]
 [SerializeField] private float dummyValue;
 ```

 + ### ForeColorAttribute
 + set's the foreground color of a field inside unity's inspector window.
 
 ```csharp
 [ForeColor(string hexColor, bool onlyThisField = false)]
 ```

 ```csharp
 // sets the foreground color into green (#00FF00) and also tells it to set to this field only
 [ForeColor("00FF00", true)]
 [SerializeField] private float dummyValue;
 ```

 + ### LabelAttribute
 + override / set the target field's label to another label.
 
 ```csharp
 [Label(string label)]
 ```

 ```csharp
 // sets the label of this field to "Hello World" 
 [Label("Hello World")]
 [SerializeField] private float dummyValue;
 ```

 + ### NotNullAttribute
 + tells the unity inspector that this field must not be null.
 
 ```csharp
 [NotNull]
 ```

 ```csharp
 // this will force unity's inspector to tell you that
 // this should be not null inorder to play / build / run
 [NotNull]
 [SerializeField] private RigidBody playerRigidBody;
 ```

---

#### Generics:
 + No Documents yet.

---

#### Helper Components:
 + No Documents yet.

---

#### Helper Utils:
 + No Documents yet.

---

#### Managers:
 + No Documents yet.

---

#### Path Finding:
 + No Documents yet.

---

#### Plane System:
 + No Documents yet.

---

#### Pooler:
 + No Documents yet.

---

#### Timer:
 + No Documents yet.

---

#### Useful Types:
 + No Documents yet.

---

***Made by CXRedix Licensed Under MIT***