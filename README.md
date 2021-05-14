# AR-alchemy-game
Это игра дополненной реальности. Игроку нужно создавать различные зелья с помощью мобильного устройства Android с камерой и напечатанных карточек (карточки можно распечатать из файла "AR alchemy cards.pdf"). Игра написана на Unity version 2020.1.4 с использованием AR библиотек от Vuforia https://developer.vuforia.com/ (Vuforia Engine Version 9.8.5). 

Готовую игру для Android устройств можно скачать по ссылке https://drive.google.com/file/d/1TwcrngMeirX1r3jh8tIPOVeUk8n2KRc1/view?usp=sharing

Основные классы:
Card - базовый класс для всех типов карточек, которые есть в игре. От него наследуются классы BaseElementClass( описывает поведение базовых элементов), CrystalCard (описывает поведение кристаллов) и CompoundElementCard (описывает поведение составных элементов).

ElementObject - базовый на котором основаны скриптовые объекты базовых элементов(BaseElementObject), составных элементов(CompoundElementObject) и кристаллов(CrystalObject), которые можно создавать в эдиторе не залезая в код.

RecipeObject - позволяет создавать рецепты для зелий.
CompoundElementRecipeObject - наследуется от RecipeObject и позволяет создавать рецепты составных элементов

ElementMixingScript - отвечает за создание составных элементов
PotionMakingScript - отвечает за создание зелий. Прикреплен к карте круга преобразования.
